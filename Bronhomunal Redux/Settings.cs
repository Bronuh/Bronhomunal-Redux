using Bronuh.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh
{
	public static class Settings
	{
		private static SettingsContainer _container = new SettingsContainer();


		public static string BotToken { get; private set; }
		public static string Sign { get; private set; } = "!";
		private static readonly string _settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BronhomunalSettings.xml";
		public static bool DEBUG = true;


		public static void Load()
		{

			Logger.Log("Загрузка настроек...");
			_container = SaveLoad.LoadObject<SettingsContainer>(_settingsPath) ?? new SettingsContainer();

			BotToken = _container.BotToken;
			Sign = _container.Sign;
			DEBUG = _container.DEBUG;

			Logger.Success("Настройки загружены");
		}

		public static void Save()
		{
			Logger.Log("Сохранение настроек...");

			_container.Sign = Sign;
			_container.BotToken = BotToken;
			_container.DEBUG = DEBUG;

			SaveLoad.SaveObject<SettingsContainer>(_container, _settingsPath);
			Logger.Success("Сохранение завершено");
		}


		[Obsolete]
		public static string GetSign()
		{
			return Sign;
		}

		public static void SetToken(string token)
		{
			BotToken = token;
			Save();
		}

	}


	[Serializable]
	public class SettingsContainer
	{
		public SettingsContainer() { }

		public string BotToken;
		public string Sign = "!";
		public bool DEBUG = true;
	}

}
