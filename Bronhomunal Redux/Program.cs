using Bronuh.Controllers;
using Bronuh.Logic;
using Bronuh.Types;
using NamedPipeWrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bronuh
{
	class Program
	{
		public static PluginController Plugins = new PluginController();
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
			InterfaceExecutor.Execute(typeof(IInitializable), "Initialize");
			InterfaceExecutor.Execute(typeof(ICommands), "InitializeCommands");
			Plugins.Initialize();
			InterfaceExecutor.Execute(typeof(ILoadable), "Load");
			
			

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
			_workingSecondChecker = new Timer(_workerSecondCallback, null, 0, 5000);

			_worker30SecCallback = new TimerCallback(Worker.Every30Sec);
			_working30SecChecker = new Timer(_worker30SecCallback, null, 0, 30000);

			_worker5MinCallback = new TimerCallback(Worker.Every5Min);
			//_working5MinChecker = new Timer(_worker5MinCallback, null, 0, 1000*60*5);
			_working5MinChecker = new Timer(_worker5MinCallback, null, 0, 1000 * 60 * 15);
			while (true)
			{
				string cmd = Console.ReadLine();
				Logger.Log("Executing console command " + cmd + "...");
				CommandsController.TryExecuteConsoleCommand(cmd).ConfigureAwait(false).GetAwaiter().GetResult();
			}
		}

		public static void SaveAll()
		{
			try
			{
				Logger.Debug("Saving all...");
				InterfaceExecutor.ExecuteStatic(typeof(PreSaveAttribute), "PreSave");
				InterfaceExecutor.Execute(typeof(ISaveable), "Save");
			}
			catch (Exception e)
			{
				Logger.Warning("Исключение вызвано в Program.SaveAll()");
				Logger.Warning(e.Message);
			}
		}
	}
}
