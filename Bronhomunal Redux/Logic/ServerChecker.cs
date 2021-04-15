using Bronuh.File;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bronuh.Logic
{
	[Serializable]
	public class ServerChecker : ISaveable, ILoadable, ICommands
	{
		public static List<ServerChecker> Checkers = new List<ServerChecker>();
		public string Host;
		public ushort Port;
		public int Timeout;
		public string Name;
		public List<ulong> Users = new List<ulong>();
		public bool IsUp;
		public int Attempts;

		private bool _isChecking = false;

		public void Load()
		{
			Logger.Log("Загрузка слушателей событий...");
			Checkers = SaveLoad.LoadObject<List<ServerChecker>>("Listeners.xml") ?? new List<ServerChecker>();
			Logger.Success("Слушатели загружены");
		}

		public void Save()
		{
			Logger.Log("Сохранение слушателей событий...");
			SaveLoad.SaveObject<List<ServerChecker>>(Checkers, "Listeners.xml");
			Logger.Success("Сохранение завершено");
		}



		/// <summary>
		/// Создает слушатель сервера, который будет периодически проверять состояние указанного сервера
		/// </summary>
		/// <param name="host">Адрес сервера</param>
		/// <param name="port">Порт сервера</param>
		/// <param name="timeout">Таймаут в секундах</param>
		/// <returns></returns>
		public static ServerChecker ListenTo(string name, string host, ushort port, int timeout = 10, int attempts = 5)
		{
			var found = Checkers.Find(c=>c.Name==name);

			if (found != null)
			{
				throw new Exception("Слушатель с таким именем уже существует");
			}

			found = Checkers.Find(c =>
			{
				return c.Host == host
				&& c.Port == port
				&& c.Timeout == timeout
				&& c.Attempts == attempts;
			});

			if (found != null)
			{
				throw new Exception("Слушатель с такими параметрами уже существует: " + found.Name);
			}

			var checker = new ServerChecker();

			checker.Host = host;
			checker.Port = port;
			checker.Timeout = timeout;
			checker.Name = name;
			checker.IsUp = false;
			checker.Attempts = attempts;

			

			Checkers.Add(checker);

			return checker;
		}



		public void Subscribe(ulong id)
		{
			if (id != 0 && !Users.Contains(id))
			{
				Users.Add(id);
			}
		}

		public void Unsubscribe(ulong id)
		{
			if (id != 0 && Users.Contains(id))
			{
				Users.Remove(id);
			}
		}


		public void Check()
		{
			if (Bot.Ready)
			{
				if (!_isChecking)
				{
					_isChecking = true;

					Logger.Success("["+Name+"] Начинаю проверку. Попыток: "+Attempts+" по "+Timeout+" секунд");

					bool up = TryCheck(Attempts);

					if (!up && IsUp)
					{
						IsUp = false;
						Notify(":no_entry: {username} Сервер {servername} **ВЫКЛЮЧЕН**");
					}
					if (up && !IsUp)
					{
						IsUp = true;
						Notify(":white_check_mark: {username} Сервер {servername} **ВКЛЮЧЕН**");
					}

					_isChecking = false;
				}
				else
				{
					Logger.Warning("[" + Name + "] Слушатель в состоянии проверки");
				}
			}
		}



		private void Notify(string text)
		{
			foreach (ulong id in Users)
			{
				var member = MembersController.FindDiscordMemberByID(id);
				var mention = member.Mention;

				member.SendMessageAsync("("+Name+"): "+text
					.Replace("{username}", mention)
					.Replace("{servername}", Host+":"+Port)
					).GetAwaiter().GetResult();
			}
		}



		private bool TryCheck(int attempts)
		{
			bool isUp = new MineStat(Host, Port, 1).ServerUp;
			Logger.Log("[" + Name + "] Осталось попыток... " + attempts);
			if (!isUp && attempts > 0)
			{
				Thread.Sleep(Timeout*1000);
				Logger.Log("[" + Name + "] Попытка не удалась. Повторное подключение..." + attempts);
				return TryCheck(attempts - 1);
			}
			else
			{
				Logger.Log("[" + Name + "] Сервер включен.");
				return isUp;
			}
		}


		public static ServerChecker Find(string search)
		{
			ServerChecker found = null;

			foreach (ServerChecker checker in Checkers)
			{
				if (checker.Name == search)
				{
					found = checker;
					break;
				}
			}

			if (found is null)
			{
				throw new Exception("Слушатель \""+search+"\" не найден");
			}

			return found;
		}


		public static void Stop(string name)
		{
			Checkers.FindAll(c => c.Name == name).ForEach(c => Checkers.Remove(c));
		}












		public void InitializeCommands()
		{
			CommandsController.AddCommand("listen", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;
				string respond = ">>> Ошибка выполнения команды: ";

				if (parts.Length == 4)
				{
					try
					{
						var checker = ListenTo(parts[1],parts[2],ushort.Parse(parts[3]));
						checker.Subscribe(m.Author.Id);

						respond = ">>> Создан слушатель '"+parts[1]+"':\n" +
						"Хост: "+parts[2]+"\n" +
						"Порт: "+parts[3]+"\n" +
						"Таймаут: "+checker.Timeout+"\n" +
						"Попытки: "+checker.Attempts;
					}
					catch (Exception e)
					{
						respond += e.Message;
						Logger.Warning(respond);
					}
				}
				else
				{
					respond = ">>> Неправильный формат команды";
				}

				await m.RespondAsync(respond);
			})
			.SetDescription("Создает новый слушатель сервера")
			.SetUsage("<command> имя_слушателя хост порт")
			.AddTags("admin","misc")
			.SetOp(true);



			CommandsController.AddCommand("listeners", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;
				string respond = ">>> Список слушателей:\n ";
				Logger.Log("Спискование слушалок...");

				foreach (var checker in Checkers)
				{
					string sub = "";
					if (checker.Users.Contains(sender.Id))
					{
						sub = " **(ПОДПИСАН)**";
					}
					respond += "\n'" + checker.Name + sub + "':\n" +
					"Хост: " + checker.Host + "\n" +
					"Порт: " + checker.Port + "\n" +
					"Таймаут: " + checker.Timeout + "\n" +
					"Попытки: " + checker.Attempts +
					"\n==============================================";
				}

				await m.RespondAsync(respond);
			})
			.AddAlias("слушатели")
			.SetDescription("Выводит список слушателей")
			.SetUsage("<command>")
			.AddTags("misc")
			.SetOp(false);


			CommandsController.AddCommand("listener", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;
				string respond = ">>> Ошибка выполнения команды: ";

				if (parts.Length > 2 && parts.Length < 5)
				{
					try
					{
						var listener = Find(parts[1]);
						var param = parts[2];

						if (param == "timeout")
						{
							if (parts.Length == 4)
							{
								int timeout = int.Parse(parts[3]);
								int before = listener.Timeout;
								listener.Timeout = Math.Max(1,Math.Min(timeout,20));
								int after = listener.Timeout;
								respond = ">>> "+listener.Name+": изменен параметр "+param+" ("+before+" -> "+after+")";
							}
							else
							{
								respond = ">>> " + listener.Name + ": " + param + " = " + listener.Timeout;
							}
						}
						else if (param == "attempts")
						{
							if (parts.Length == 4)
							{
								int attempts = int.Parse(parts[3]);
								int before = listener.Attempts;
								listener.Attempts = Math.Max(1, Math.Min(attempts, 60));
								int after = listener.Attempts;
								respond = ">>> " + listener.Name + ": изменен параметр " + param + " (" + before + " -> " + after + ")";
							}
							else
							{
								respond = ">>> " + listener.Name + ": "+param+" = "+listener.Attempts;
							}
						}
						else if (param == "remove")
						{
							if (parts.Length == 3)
							{
								listener.Notify("Слушатель "+listener.Name+" удален");
								Checkers.Remove(listener);
								respond = ">>> " + listener.Name + ": удален";
							}
							else
							{
								respond = ">>> Параметр remove не принимает значений";
							}
						}
						else
						{
							respond = ">>> Неизвестный параметр "+param;
						}
					}
					catch (Exception e)
					{
						respond += e.Message;
						Logger.Warning(respond);
					}
				}
				else
				{
					respond = ">>> Неправильный формат команды";
				}

				await m.RespondAsync(respond);
			})
			.SetDescription("Меняет параметры слушателя:\n" +
			"timeout - время ожидания ответа в секундах при каждой попытке подключения (min: 1, max:20, def: 10)\n" +
			"attempts - количество попыток подключения при периодической проверке (min: 1, max: 60, def: 5)")
			.SetUsage("<command> имя_слушателя параметр значение, либо <command> имя_слушателя параметр")
			.AddTags("admin", "misc")
			.SetOp(true);


			CommandsController.AddCommand("unsubscribe", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;
				string respond = ">>> Ошибка выполнения команды: ";

				if (parts.Length == 2)
				{
					try
					{
						var checker = Find(parts[1]);
						checker.Unsubscribe(sender.Id);
						respond = ">>> Отписан от "+checker.Name;
					}
					catch (Exception e)
					{
						respond += e.Message;
						Logger.Warning(respond);
					}
				}
				else
				{
					respond = ">>> Неправильный формат команды";
				}

				await m.RespondAsync(respond);
			})
			.AddAliases("отписаться")
			.SetDescription("Отписывает пользователя от слушателя")
			.SetUsage("<command> имя_слушателя")
			.AddTags("misc");


			CommandsController.AddCommand("subscribe", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;
				string respond = ">>> Ошибка выполнения команды: ";

				if (parts.Length == 2)
				{
					try
					{
						var checker = Find(parts[1]);
						checker.Subscribe(sender.Id);
						respond = ">>> Подписан на " + checker.Name;
					}
					catch (Exception e)
					{
						respond += e.Message;
						Logger.Warning(respond);
					}
				}
				else
				{
					respond = ">>> Неправильный формат команды";
				}

				await m.RespondAsync(respond);
			})
			.AddAliases("подписаться")
			.SetDescription("Подписывает пользователя на слушателя")
			.SetUsage("<command> имя_слушателя")
			.AddTags("misc");
		}
	}
}
