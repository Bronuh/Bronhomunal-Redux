using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bronuh.Controllers
{
	public class PluginController : IPluginHost
	{
		List<IPlugin> _plugins;
		static string PluginsPath = AppDomain.CurrentDomain.BaseDirectory + "Plugins\\";

		public void Initialize()
		{
			Logger.Log("=======================================================================================================");
			Logger.Log("Загрузка плагинов...");
			Logger.Log("=======================================================================================================");
			if (!Directory.Exists(PluginsPath))
			{
				Console.WriteLine("Создана директория: " + PluginsPath);
				Directory.CreateDirectory(PluginsPath);
			}

			LoadPlugins(PluginsPath);

			Logger.Log("=======================================================================================================");
			Logger.Log("Загрузка плагинов завершена");
			Logger.Log("=======================================================================================================");
		}

		public bool Register(IPlugin plugin)
		{
			return true;
		}

		private void LoadPlugins(string path)
		{
			string[] pluginFiles = Directory.GetFiles(path, "*.dll");
			this._plugins = new List<IPlugin>();

			foreach (string pluginPath in pluginFiles)
			{
				Type objType = null;
				try
				{
					// пытаемся загрузить библиотеку
					Assembly assembly = Assembly.LoadFrom(pluginPath);
					if (assembly != null)
					{
						objType = assembly.GetType(Path.GetFileNameWithoutExtension(pluginPath) + ".Plugin");
					}
				}
				catch(Exception e)
				{
					Logger.Warning("Исключение вызвано в PluginController.LoadPlugins()");
					Logger.Error(e.Message);
					continue;
				}
				try
				{
					if (objType != null)
					{
						var _plugin = (IPlugin)Activator.CreateInstance(objType);
						this._plugins.Add(_plugin);
						this._plugins[this._plugins.Count - 1].Host = this;
						this._plugins[this._plugins.Count - 1].Run();
						Logger.Log(_plugin.PluginName + " загружен");
					}
				}
				catch (Exception e)
				{
					Logger.Warning("Исключение вызвано в PluginController.LoadPlugins()");
					Logger.Error(e.Message);
					continue;
				}
			}
		}
	}

	public interface IPlugin
	{
		IPluginHost Host { get; set; }
		string PluginName { get; set; }
		string PluginDescription { get; set; }

		void Run();
	}

	public interface IPluginHost
	{
		public bool Register(IPlugin plugin);
	}
}
