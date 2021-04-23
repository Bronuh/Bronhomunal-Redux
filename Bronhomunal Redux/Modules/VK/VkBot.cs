using Bronhomunal_VK;
using System;
using System.Collections.Generic;
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

			Console.ReadLine();
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
					respond += memeText + "\n\n";
				}
				SendMessage(respond, chatId);
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
		}
	}
}
