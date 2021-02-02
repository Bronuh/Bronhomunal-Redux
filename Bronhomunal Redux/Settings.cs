using Bronuh.File;
using System;

namespace Bronuh
{
	public class Settings : ISaveable, ILoadable
	{
		private static SettingsContainer _container = new SettingsContainer();

		public static bool ServerStatus { get; set; } = false;
		public static string BotToken { get; private set; }
		public static string Sign { get; private set; } = "!";
		private static readonly string _settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BronhomunalSettings.xml";
		public static bool DEBUG = false;
		public static int LaunchCount = 0;

		public void Load()
		{
			Logger.Log("Загрузка настроек...");
			_container = SaveLoad.LoadObject<SettingsContainer>(_settingsPath) ?? new SettingsContainer();

			BotToken = _container.BotToken;
			Sign = _container.Sign;
			DEBUG = _container.DEBUG;
			LaunchCount = _container.LaunchCount;
			ServerStatus = _container.ServerStatus;

			LaunchCount++;
			Logger.Success("Настройки загружены");
		}

		public void Save()
		{
			Logger.Log("Сохранение настроек...");

			_container.Sign = Sign;
			_container.BotToken = BotToken;
			_container.DEBUG = DEBUG;
			_container.LaunchCount = LaunchCount;
			_container.ServerStatus = ServerStatus;

			SaveLoad.SaveObject<SettingsContainer>(_container, _settingsPath);
			Logger.Success("Сохранение завершено");
		}

		public static void SetToken(string token)
		{
			BotToken = token;
			new Settings().Save();
		}
	}

	[Serializable]
	public class SettingsContainer
	{
		public SettingsContainer() { }

		public int LaunchCount = 0;
		public string BotToken;
		public string Sign = "!";
		public bool DEBUG = false;
		public bool ServerStatus = false;
	}
}
