using System;
using System.Threading;
using System.Threading.Tasks;
using Bronuh.Events;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus;
using DSharpPlus.Entities;
using NamedPipeWrapper;

namespace Bronuh
{
	class Program
	{
		public static string Suffix = "";
		public static string Prefix = "";

		public static NamedPipeClient<string> Client = new NamedPipeClient<string>("BronhomunalPipe");
		public static NamedPipeServer<string> Server = new NamedPipeServer<string>("LauncherPipe");
		static void Main()
		{

			Logger.Log("Загрузка...");

			#if DEBUG
				Suffix = " [DEBUG]";
				Prefix = "!";
			#endif

			InterfaceExecutor.Execute(typeof(ILoadable), "Load");
			InterfaceExecutor.Execute(typeof(IInitializable), "Initialize");


			Logger.Log("Инициализация бота...");

			new Thread(new ThreadStart(() =>
			{
				Bot.Initialize(Settings.BotToken);
			})).Start();


			Logger.Log("Подключение к Pipe серверу...");

			
			
			

			var outer = Task.Factory.StartNew(() =>      // внешняя задача
			{
				Client.ServerMessage += (connection, message) =>
				{
					Logger.Warning("Получено сообщение от лаунчера: " + message);
					if (message == "SaveAndExit")
					{
						SaveAll();
						Environment.Exit(0);
					}
				};
				Client.Start();
				Server.Start();
				Server.PushMessage("Connected");
			});

			while (true)
			{
				string cmd = Console.ReadLine();
				CommandsController.TryExecuteConsoleCommand(cmd).ConfigureAwait(false).GetAwaiter().GetResult();
			}
			
		}


		public static void SaveAll()
		{
			InterfaceExecutor.Execute(typeof(ISaveable), "Save");
		}


	}
}
