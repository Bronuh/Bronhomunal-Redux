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


		public static void SaveAll()
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
			
		}
	}
}
