using Bronuh.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh
{
	public static class Settings
	{
		private static SettingsContainer _container = new SettingsContainer();


		public static string BotToken;
		public static string Sign = "!";
		private static string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BronhomunalSettings.xml";

		public static void Load()
		{

			Logger.Log("Загрузка настроек...");
			_container = SaveLoad.LoadObject<SettingsContainer>(SettingsPath) ?? new SettingsContainer();

			BotToken = _container.BotToken;
			Sign = _container.Sign;

			Logger.Success("Настройки загружены");
		}

		public static void Save()
		{
			Logger.Log("Сохранение настроек...");

			_container.Sign = Sign;
			_container.BotToken = BotToken;

			SaveLoad.SaveObject<SettingsContainer>(_container, SettingsPath);
			Logger.Success("Сохранение завершено");
		}


		[Obsolete]
		public static string GetSign()
		{
			return Sign;
		}

	}


	[Serializable]
	public class SettingsContainer
	{
		public SettingsContainer() { }

		public string BotToken;
		public string Sign = "!";
	}

}
