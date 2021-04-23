using NamedPipeWrapper;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Launcher
{
	class Program
	{
		public enum Status
		{
			WORKING,
			STOPPED,
			UPDATING
		}
		static Process bot = null;

		static DirectoryInfo Current = new DirectoryInfo(Directory.GetCurrentDirectory());
		static DirectoryInfo Root = Current.Parent.Parent.Parent.Parent;
		static DirectoryInfo PluginsRoot = Current.Parent.Parent.Parent.Parent.Parent.Parent;
		static DirectoryInfo AssetsRoot = Current.Parent.Parent.Parent;

#if DEBUG
		static string BuildPath = @"bin\debug\net5.0\";
#else
		static string BuildPath = @"bin\release\net5.0\";
#endif
		static string TargetApp = "Bronhomunal Redux";
		static string TargetExe = TargetApp + ".exe";
		static string TargetDll = TargetApp + ".dll";

		static FileInfo CurrentExe, CurrentDll;

		static NamedPipeServer<string> Server = new NamedPipeServer<string>("BronhomunalPipe");
		public static NamedPipeClient<string> Client = new NamedPipeClient<string>("LauncherPipe");

		static NamedPipeConnection<string, string> Bot;
		
		static Status CurrentStatus = Status.STOPPED;

		static void Main()
		{
			if (!Directory.Exists("Plugins\\"))
			{
				Console.WriteLine("Создана директория: " + "Plugins\\");
				Directory.CreateDirectory("Plugins\\");
			}
			PluginsRoot = new DirectoryInfo(PluginsRoot.FullName + @"\Libs\BronhomunalPlugins\");

			if (!Directory.Exists("Assets\\"))
			{
				Console.WriteLine("Создана директория: " + "Assets\\");
				Directory.CreateDirectory("Assets\\");
			}
			AssetsRoot = new DirectoryInfo(AssetsRoot.FullName + @"\Assets");

			Console.WriteLine("Подготовка...");
			Prepare();

			
			TimerCallback workingCallback = new TimerCallback((o) =>
			{
				if (!CheckWorking())
				{
					if (CurrentStatus == Status.WORKING)
					{
						Console.WriteLine("процессов не найдено");
						Start();
					}
				}
			});

			TimerCallback updateCallback = new TimerCallback((o) =>
			{
				FileInfo
					targetDll = new FileInfo(Root + @"\Bronhomunal Redux\" + BuildPath + TargetDll),
					targetExe = new FileInfo(Root + @"\Bronhomunal Redux\" + BuildPath + TargetExe);

				if (File.GetLastWriteTime(targetDll.FullName) != File.GetLastWriteTime(CurrentDll.FullName)
				|| File.GetLastWriteTime(targetExe.FullName) != File.GetLastWriteTime(CurrentExe.FullName))
				{
					if (CurrentStatus != Status.UPDATING)
					{
						CurrentStatus = Status.UPDATING;
						Server.PushMessage("SaveAndExit");
						Thread.Sleep(3000);

						Console.WriteLine("UPDATING!!!\n\n\n\n\n\n");
						try
						{
							UpdateAll();
						}
						catch (Exception e)
						{
							Console.WriteLine(e.Message);
						}

						CurrentStatus = Status.WORKING;
					}
				}
			});

			Console.WriteLine("Запуск таймеров");
			Timer workingChecker = new Timer(workingCallback, null, 0, 1000);
			Timer updateChecker = new Timer(updateCallback, null, 0, 1000);

			Console.ReadLine();
		}

		public static bool CheckWorking()
		{
			Process[] pr = Process.GetProcessesByName(TargetApp);
			return pr.Length > 0;
		}

		public static void Start()
		{
			Console.WriteLine("Запуск бота...");
			ProcessStartInfo start = new ProcessStartInfo(Current + "\\" + TargetExe);
			Console.WriteLine(start.FileName);
			start.UseShellExecute = true;
			bot = Process.Start(start);
		}

		public static void Prepare()
		{
			bool exeFound = false;
			bool dllFound = false;

			var files = Current.GetFiles();

			UpdateAll();

			CurrentExe = new FileInfo(Current + "\\" + TargetExe);
			CurrentDll = new FileInfo(Current + "\\" + TargetDll);

			Console.WriteLine("Подготовка Pipe сервера...");

			var outer = Task.Factory.StartNew(() =>
			{
				Client.ServerMessage += (connection, message) =>
				{
					Console.WriteLine("Получено сообщение от бота: " + message);
					if (message == "Shutdown")
					{
						CurrentStatus = Status.STOPPED;
						Environment.Exit(0);
					}
				};

				Server.ClientConnected += (connection) =>
				{
					Bot = connection;
					Console.WriteLine("Подключен клиент");
					Server.PushMessage("Eet som shiet");
				};
				Client.Start();
				Server.Start();

			});

			Console.WriteLine("Подготовка завершена");
			CurrentStatus = Status.WORKING;
		}

		public static void UpdateAll()
		{
			foreach (FileInfo file in new DirectoryInfo(Root.FullName + "\\Bronhomunal Redux\\" + BuildPath).GetFiles())
			{
				if (file.Name.EndsWith(".exe") || file.Name.EndsWith(".dll") || file.Name.EndsWith(".json") || file.Name.EndsWith(".pdb"))
				{
					UpdateFile(file.FullName);
				}
			}

			Utils.CheckDirectory("Assets");
			Utils.ForceDeleteDirectory("Assets");
			Utils.DirectoryCopy(Root.FullName + "\\Bronhomunal Redux\\" + BuildPath+"Assets", "Assets",true);

			foreach (var dir in new DirectoryInfo(PluginsRoot.FullName).GetDirectories())
			{
				if(!dir.Name.StartsWith("."))
				{
					foreach (FileInfo file in new DirectoryInfo(dir.FullName+"\\" + BuildPath).GetFiles())
					{
						if (file.Name.EndsWith(".dll"))
						{
							UpdatePlugin(file.FullName);
						}
					}
				}
			}
		}

		public static void UpdateFile(string target)
		{
			Console.WriteLine("Обновление файла " + target);
			FileInfo foundLocal = null, found = null;

			if (!File.Exists(target))
			{
				return;
			}

			found = new FileInfo(target);

			foreach (FileInfo file in Current.GetFiles())
			{
				if (file.Name.ToLower() == found.Name.ToLower())
				{
					foundLocal = file;
					break;
				}
			}

			if (foundLocal != null)
			{
				Console.WriteLine("Создание бэкапа...");
				if (File.Exists(foundLocal.FullName + ".bak"))
				{
					File.Delete(foundLocal.FullName + ".bak");
				}

				File.Move(foundLocal.FullName, foundLocal.FullName + ".bak");
			}

			try
			{
				File.Copy(target, Current + "\\" + found.Name);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Обновление завершено");
		}

		public static void UpdatePlugin(string target)
		{
			Console.WriteLine("Обновление плагина " + target);
			FileInfo foundLocal = null, found = null;

			if (!File.Exists(target))
			{
				return;
			}

			found = new FileInfo(target);

			foreach (FileInfo file in new DirectoryInfo(Current.FullName+"\\Plugins").GetFiles())
			{
				if (file.Name.ToLower() == found.Name.ToLower())
				{
					foundLocal = file;
					break;
				}
			}

			if (foundLocal != null)
			{
				Console.WriteLine("Удаление старого файла...");
				if (File.Exists(foundLocal.FullName + ".bak"))
				{
					File.Delete(foundLocal.FullName + ".bak");
				}

				if (File.Exists(foundLocal.FullName))
				{
					File.Delete(foundLocal.FullName);
				}

				//File.Move(foundLocal.FullName, foundLocal.FullName + ".bak");
			}

			try
			{
				File.Copy(target, Current.FullName + "\\Plugins\\" + found.Name);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Обновление завершено");
		}
	}
}
