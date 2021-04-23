using Bronuh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
	public class Utils
	{
		public static void CheckDirectory(string dir)
		{
			if (!Directory.Exists(dir + "\\"))
			{
				Logger.Log("Создана директория: " + dir + "\\");
				Directory.CreateDirectory(dir + "\\");
			}
		}

		public static void ForceDeleteDirectory(string path)
		{
			var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

			foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
			{
				info.Attributes = FileAttributes.Normal;
			}

			directory.Delete(true);
		}
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool overwrite)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the destination directory doesn't exist, create it.       
			Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			string exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
			foreach (FileInfo file in files)
			{
				string tempPath = Path.Combine(destDirName, file.Name);
				Logger.Log("Копирование: " + file.Name);
				if (file.Name != exeName)
				{
					try
					{
						file.CopyTo(tempPath, overwrite);
					}
					catch (Exception e)
					{
						Logger.Error(e.Message);
					}
				}
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (true)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string tempPath = Path.Combine(destDirName, subdir.Name);
					if (subdir.Name != "CurrentVersion" && subdir.Name != "TempVersion" && subdir.Name != "OldVersion")
					{
						DirectoryCopy(subdir.FullName, tempPath, true);
					}
				}
			}
		}
	}
}
