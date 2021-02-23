using Bronuh;
using Bronuh.Controllers;
using System;

namespace TestPlugin
{
	public class Plugin : IPlugin
	{
		IPluginHost _Host;
		public IPluginHost Host
		{
			get { return _Host; }
			set
			{
				_Host = value;
				_Host.Register(this);
			}
		}
		public string PluginName { get; set; } = "Test Plugin";
		public string PluginDescription { get; set; } = "Тестируемый плагин. Прям совсем тестируемый.";

		public void Run()
		{
			Logger.Success($"{PluginName} успешно загружен");
		}
	}
}
