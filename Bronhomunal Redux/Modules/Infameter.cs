using Bronuh.File;
using Bronuh.Logic;
using Bronuh.Types;
using System;

namespace Bronuh.Modules
{
	public class Infameter : ISaveable, ILoadable
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

		public static Infa FindInfo(String search, Member sender)
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
					Value = Math.Round(new Random().NextDouble() * 100),
					Author = sender.Id
				};

				if (found.Value == 0)
				{
					sender.GiveAchievement("no");
				}

				if (found.Value == 50)
				{
					sender.GiveAchievement("maybe");
				}

				if (found.Value == 100)
				{
					sender.GiveAchievement("yes");
				}

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
		public ulong Author;

		public static Infa CheckInfo(String infa, Member sender)
		{
			String text = infa.ToLower().Replace("?", "").Replace("!", "").Replace(".", "").Replace("ё", "е").Trim().Replace("   ", "").Replace("  ", " ");
			return Modules.Infameter.FindInfo(text, sender);
		}
	}
}
