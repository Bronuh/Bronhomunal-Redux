using Bronhomunal_VK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Bronuh.Modules.VK
{
	class VkBot
	{

		public static VkApi API;
		public static List<String> Files = new List<string>();

		public static void StartVkBot()
		{
			Console.WriteLine("Авторизация вк с токеном: "+Settings.VKToken);
			API = new VkApi();

			API.Authorize(new ApiAuthParams()
			{
				AccessToken = Settings.VKToken
			});
			Initialize();

			new Thread(() => {
				Upd();
			}).Start();

			

			//Console.ReadLine();
		}

		static void Upd()
		{
			while (true) // Бесконечный цикл, получение обновлений
			{
				try
				{
					var s = API.Groups.GetLongPollServer(204006161);
					var poll = API.Groups.GetBotsLongPollHistory(
					   new BotsLongPollHistoryParams()
					   { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });
					if (poll?.Updates == null) continue; // Проверка на новые события


					foreach (var a in poll.Updates)
					{
						if (a.Type == GroupUpdateType.MessageNew)
						{
							string userMessage = a.MessageNew.Message.Text ?? "NULL";//a.Message.Body?.ToLower() ?? "NULL";
							long? userID = a.MessageNew.Message.UserId;
							Console.WriteLine("Всосал сообщеньку: " + userMessage);
							if (userMessage.StartsWith("!"))
							{
								HandleCommand(userMessage, a.MessageNew.Message.PeerId - 2000000000);
							}

						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

			}
		}

		public static void HandleCommand(string message, long? chatId)
		{
			Console.WriteLine("Handling command...");
			message = message.Replace("ё","е");
			if (message.StartsWith("!memes") || message.StartsWith("!мемы") || message.StartsWith("!мемасики"))
			{
				Console.WriteLine("Memes list requested (" + Memes.memes.Count + " memes)");
				string respond = "Мемасики:\n";

				foreach (var meme in Memes.memes)
				{
					string memeText = "!" + meme.Name + ": ";
					foreach (var alias in meme.Aliases)
					{
						memeText += alias;
						if (meme.Aliases.Last() != alias)
						{
							memeText += ", ";
						}
					}
					respond += memeText + "\n";
				}
				SendMessage(respond, chatId);
				return;
			}

			if (message.StartsWith("!randommeme") || message.StartsWith("!random") || message.StartsWith("!рандом"))
			{
				Console.WriteLine("Random meme requested (" + Files.Count + " memes)");

				DirectoryInfo Current = new DirectoryInfo(Directory.GetCurrentDirectory());
				DirectoryInfo Root = Current.Parent.Parent.Parent.Parent;
				Files.Clear();
				DirectoryInfo dir = new DirectoryInfo(Path.Combine(Root.FullName, "Bronhomunal Redux", "Assets", "RandomMemes"));
				Array.ForEach(dir.GetFiles(), (f) => { Files.Add(f.Name); });

				var memePath = Path.Combine(Environment.CurrentDirectory, "Assets", "RandomMemes", Files.GetRandom());
				//SendMessage(respond, chatId);
				SendDoc(memePath,chatId);
				return;
			}

			if (message.StartsWith("!file") || message.StartsWith("!файл") || message.StartsWith("!мем"))
			{
				Console.WriteLine("Concrete meme requested (" + Files.Count + " memes)");

				var parts = message.Split(' ');

				if (parts.Length<1)
				{
					return;
				}

				var mems = new List<String>(Files.Where(f => f.ToLower().StartsWith(message.Replace(parts[0]+" ","").ToLower())));

				Console.WriteLine("Found " + mems.Count + " files");
				var memePath = Path.Combine(Environment.CurrentDirectory, "Assets", "RandomMemes", mems.GetRandom());

				//SendMessage(respond, chatId);
				SendDoc(memePath, chatId);
				return;
			}

			if (message.StartsWith("!quote") || message.StartsWith("!цитата"))
			{

				var parts = message.Split(' ');

				if (parts.Length < 1)
				{
					return;
				}

				//SendMessage(respond, chatId);
				SendMessage(RSS.GetRandomQuote(), chatId);
				return;
			}

			if (message.StartsWith("!joke") || message.StartsWith("!шутка"))
			{

				var parts = message.Split(' ');

				if (parts.Length < 1)
				{
					return;
				}
				SendMessage(RSS.GetRandomJoke(), chatId);
				return;
			}

			if (message.StartsWith("!anecdote") || message.StartsWith("!анекдот"))
			{

				var parts = message.Split(' ');

				if (parts.Length < 1)
				{
					return;
				}
				string[] anecData = RSS.GetRandomAnecdote();
				SendAnek(anecData[1],anecData[0], chatId);
				return;
			}

			if (message.StartsWith("!comics") || message.StartsWith("!комикс"))
			{

				var parts = message.Split(' ');

				if (parts.Length < 1)
				{
					return;
				}

				//SendMessage(respond, chatId);
				SendPngFromUrl(RSS.GetRandomComicsLink(), chatId);
				return;
			}


			try
			{
				var meme = Memes.FindMeme(message);
				if (meme != null)
				{
					SendMeme(meme.GetMemeFile(), chatId);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public static void SendMeme(string path, long? chatId)
		{
			Console.WriteLine("Sending meme " + path);
			Random rnd = new Random();
			API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { Photo(path) }
			});
		}

		public static void SendAnek(string url, string text, long? chatId)
		{
			Random rnd = new Random();
			API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Message = text,
				Attachments = new List<MediaAttachment> { PhotoFromUrlJpg(url) }
			});
		}

		public static void SendDoc(string path, long? chatId)
		{
			Console.WriteLine("Sending doc " + path);
			Random rnd = new Random();
			API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { Document(path) }
			});
		}

		public static long SendMessage(string message, long? chatId)
		{
			Random rnd = new Random();
			return API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Message = message
			});
		}

		public static long SendPngFromUrl(string url, long? chatId)
		{
			Random rnd = new Random();
			return API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { PhotoFromUrl(url) }
			});
		}

		public static long SendJpgFromUrl(string url, long? chatId)
		{
			Random rnd = new Random();
			return API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { PhotoFromUrlJpg(url) }
			});
		}

		public static MediaAttachment PhotoFromUrl(string url)
		{
			Console.WriteLine("Getting photo from " + url);
			var wc = new WebClient();
			wc.DownloadFile(url,"temp.png");
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "temp.png"));
			var photo = API.Photo.SaveMessagesPhoto(result);//.SaveMessagesPhoto(url);
			wc.Dispose();
			
			return photo.FirstOrDefault();
		}

		public static MediaAttachment PhotoFromUrlJpg(string url)
		{
			Console.WriteLine("Getting photo from " + url);
			var wc = new WebClient();
			wc.DownloadFile(url, "temp.jpg");
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "temp.jpg"));
			var photo = API.Photo.SaveMessagesPhoto(result);//.SaveMessagesPhoto(url);
			wc.Dispose();
			return photo.FirstOrDefault();
		}

		public static MediaAttachment Photo(string path)
		{
			Console.WriteLine("Getting photo from " + path);
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var wc = new WebClient();
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, path));
			var photo = API.Photo.SaveMessagesPhoto(result);
			wc.Dispose();
			return photo.FirstOrDefault();
		}

		public static MediaAttachment Document(string path)
		{
			Console.WriteLine("Getting document from " + path);
			var uploadServer = API.Docs.GetMessagesUploadServer(88798690);
			var wc = new WebClient();
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, path));
			var doc = API.Docs.Save(result,Path.GetFileNameWithoutExtension(path),tags: null);
			wc.Dispose();
			List<VkNet.Model.Attachments.MediaAttachment> _attachments = new List<VkNet.Model.Attachments.MediaAttachment>();
			foreach (var a in doc)
				_attachments.Add(a.Instance);
			return _attachments.FirstOrDefault();
		}


		public static void Initialize()
		{
			Meme.Initialize();
			Memes.AddMeme(new Meme("fuck"))
				.AddAliases("пиздец", "oof", "больно", "бля");

			Memes.AddMeme(new Meme("killfrog"))
				.AddAlias("киллфрог");

			Memes.AddMeme(new Meme("hmm"))
				.AddAliases("хмм", "думает", "думоет");

			Memes.AddMeme(new Meme("goodenough"))
				.AddAliases("итаксойдет", "похуй", "норм", "сойдет");

			Memes.AddMeme(new Meme("work"))
				.AddAliases("работай", "dosomething");

			Memes.AddMeme(new Meme("dumb"))
				.AddAliases("тупой", "ыыы", "гы");

			Memes.AddMeme(new Meme("omg"))
				.AddAliases("ничоси", "ничосе", "хуясе", "хуяси", "охуеть", "охренеть");

			Memes.AddMeme(new Meme("uuu"))
				.AddAliases("ууу", "ъуъ", "уъу");

			Memes.AddMeme(new Meme("plan"))
				.AddAliases("план", "надежно", "охуенныйплан");

			Memes.AddMeme(new Meme("nou"))
				.AddAliases("нетты", "ты");

			Memes.AddMeme(new Meme("genius"))
				.AddAliases("outstanding", "гениально", "гений");

			Memes.AddMeme(new Meme("catwork"))
				.AddAliases("котработай");

			Memes.AddMeme(new Meme("catnotwork"))
				.AddAliases("котнеработай", "самаработай");

			Memes.AddMeme(new Meme("hotsucatnotwork"))
				.AddAliases("самработай");

			Memes.AddMeme(new Meme("a"))
				.AddAliases("а");

			Memes.AddMeme(new Meme("urahara"))
				.AddAliases("урахара");

			Memes.AddMeme(new Meme("tom"))
				.AddAliases("том");

			Memes.AddMeme(new Meme("hue"))
				.AddAliases("хее");

			Memes.AddMeme(new Meme("why"))
				.AddAliases("зачем");

			Memes.AddMeme(new Meme("because"))
				.AddAliases("затем", "патамушта","зотем");

			Memes.AddMeme(new Meme("jpeg"))
				.AddAliases("jpg","жипег","шакал","жпг");

			Memes.AddMeme(new Meme("naruto"))
				.AddAliases("наруто", "нарик");

			Memes.AddMeme(new Meme("titans"))
				.AddAliases("титосы", "титаны");

			Memes.AddMeme(new Meme("play"))
				.AddAliases("катать", "кататьтитосы");


			Memes.AddMeme(new Meme("luck"))
				.AddAliases("повезло", "удача");

			Memes.AddMeme(new Meme("wtf"))
				.AddAliases("чзх", "втф");

			Memes.AddMeme(new Meme("letsgo"))
				.AddAliases("погнали", "го");

			Memes.AddMeme(new Meme("plsno"))
				.AddAliases("нинада", "можноненадо");

			Memes.AddMeme(new Meme("great"))
				.AddAliases("великолепно", "10", "10/10");

			Memes.AddMeme(new Meme("rage"))
				.AddAliases("ярость", "ненависть");
		}
	}
}
