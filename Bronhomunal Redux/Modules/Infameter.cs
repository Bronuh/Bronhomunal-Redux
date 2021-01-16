using Bronuh.File;
using Bronuh.Logic;
using Bronuh.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Modules
{
	class Infameter
	{

		public static Sequence<Infa> Infos = new Sequence<Infa>();

		public static void Load()
		{
			Logger.Log("Загрузка списка псевдонимов...");
			Infos = SaveLoad.LoadObject<Sequence<Infa>>("Infos.xml") ?? new Sequence<Infa>();
			Logger.Success("Псевдонимы загружены");
		}

		public static void Save()
		{
			Logger.Log("Сохранение списка псевдонимов...");
			SaveLoad.SaveObject<Sequence<Infa>>(Infos, "Infos.xml");
			Logger.Success("Сохранение завершено");
		}

        public static Infa FindInfo(String search)
        {
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
                Save();
            }
            return found;
        }

    }
}
