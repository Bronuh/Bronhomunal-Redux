using System;
using System.Threading;
using System.Threading.Tasks;
using Bronuh.Events;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bronuh
{
	class Program
	{
		static void Main()
		{
			Logger.Log("Загрузка...");

			MembersController.Load();
			AliasesController.Load();
			Settings.Load();

			Logger.Log("Инициализация бота...");

			new Thread(new ThreadStart(() =>
			{
				Bot.Initialize(Settings.BotToken);
			})).Start();

			Initialize();

			while (true)
			{
				string cmd = Console.ReadLine();
				CommandsController.TryExecuteConsoleCommand(cmd).ConfigureAwait(false).GetAwaiter().GetResult();
			}
			
		}


		private static void SaveAll()
		{
			MembersController.Save();
			AliasesController.Save();
			Settings.Save();
			Infameter.Save();
			Logger.SaveLog();
		}



		public static void Initialize()
		{
			InitializeCommands();
		}



		public static void InitializeCommands()
		{
			CommandsController.AddCommand("shutdown", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOP)
				{
					SaveAll();
					System.Diagnostics.Process.Start("cmd", "/c shutdown /s /t 0");
				}
			}).SetOp(true);

			CommandsController.AddCommand("kill", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOP)
				{
					SaveAll();
					Environment.Exit(0);
				}
			}).SetOp(true);

			CommandsController.AddCommand("op", async e =>
			{
				string[] parts = e.Text.Split(' ');

				if (Bot.Ready)
				{
					try
					{
						Member member = MembersController.FindMember(parts[1]);
						member.IsOP = true;
						Logger.Success("Выданы права администратора пользователю " + member.DisplayName);
					}
					catch (Exception ex)
					{
						Logger.Error(ex.Message);
					}
				}
				else
				{
					Logger.Warning("Бот еще не готов");
				}
			}).SetOp(true);

			CommandsController.AddCommand("deop", async e =>
			{
				string[] parts = e.Text.Split(' ');

				if (Bot.Ready)
				{
					try
					{
						Member member = MembersController.FindMember(parts[1]);
						member.IsOP = false;
						Logger.Success("Пользователь лишен прав администратора" + member.DisplayName);
					}
					catch (Exception ex)
					{
						Logger.Error(ex.Message);
					}
				}
				else
				{
					Logger.Warning("Бот еще не готов");
				}
			}).SetOp(true);
		}
	}
}
