using Bronuh.Logic;
using NamedPipeWrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bronuh
{
	class Program
	{
		private static TimerCallback 
			_workerSecondCallback,
			_worker30SecCallback,
			_worker5MinCallback;
		private static Timer 
			_workingSecondChecker,
			_working30SecChecker,
			_working5MinChecker;
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

			_workerSecondCallback = new TimerCallback(Worker.EverySecond);
			_workingSecondChecker = new Timer(_workerSecondCallback, null, 0, 1000);

			_worker30SecCallback = new TimerCallback(Worker.Every30Sec);
			_working30SecChecker = new Timer(_worker30SecCallback, null, 0, 30000);

			_worker5MinCallback = new TimerCallback(Worker.Every5Min);
			_working5MinChecker = new Timer(_worker5MinCallback, null, 0, 1000*60*5);

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
