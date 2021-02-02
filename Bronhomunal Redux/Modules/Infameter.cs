using Bronuh.File;
using Bronuh.Logic;
using System;

namespace Bronuh.Modules
{
	class Infameter : ISaveable, ILoadable
	{
		private static Sequence<Infa> Infos = new Sequence<Infa>();
		private static bool _initialized = false;

		public void Load()
		{
			Logger.Log("Загрузка списка инфы...");
			Infos = SaveLoad.LoadObject<Sequence<Infa>>("Infos.xml") ?? new Sequence<Infa>();
			Logger.Success("Инфа загружена");
		}

		public void Save()
		{
			Logger.Log("Сохранение списка инфы...");
			SaveLoad.SaveObject<Sequence<Infa>>(Infos, "Infos.xml");
			Logger.Success("Сохранение завершено");
		}

		public static Infa FindInfo(String search)
		{
			if (!_initialized)
			{
				new Infameter().Load();
				_initialized = true;
			}
			Infa found = null;
			foreach (Infa infa in Infos)
			{
				if (infa.Text == search)
				{
					found = infa;
				}
			}

			if (found == null)
			{
				found = new Infa()
				{
					Text = search,
					Value = new Random().NextDouble() * 100
				};
				Infos.Add(found);
				new Infameter().Save();
			}
			return found;
		}
	}

	[Serializable]
	public class Infa
	{
		public string Text;
		public double Value;

		public static Infa CheckInfo(String infa)
		{
			String text = infa.ToLower().Replace("?", "").Replace("!", "").Replace(".", "").Replace("ё", "е").Trim().Replace("   ", "").Replace("  ", " ");
			return Modules.Infameter.FindInfo(text);
		}
	}
}
