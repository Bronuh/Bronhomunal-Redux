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


			InterfaceExecutor.Execute("ILoadable", "Load");


			Logger.Log("Инициализация бота...");

			new Thread(new ThreadStart(() =>
			{
				Bot.Initialize(Settings.BotToken);
			})).Start();


			while (true)
			{
				string cmd = Console.ReadLine();
				CommandsController.TryExecuteConsoleCommand(cmd).ConfigureAwait(false).GetAwaiter().GetResult();
			}
			
		}


		public static void SaveAll()
		{
			InterfaceExecutor.Execute("ISaveable", "Save");
		}


	}
}
