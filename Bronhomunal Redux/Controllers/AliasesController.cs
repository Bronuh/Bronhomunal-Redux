using Bronuh.File;
using Bronuh.Logic;
using Bronuh.Types;
using System;
using System.Collections.Generic;

namespace Bronuh
{
	public class AliasesController : ISaveable, ILoadable
	{
		public static Sequence<Alias> Aliases { get; private set; } = new Sequence<Alias>();

		public void Load()
		{
			Logger.Log("Загрузка списка псевдонимов...");
			Aliases = SaveLoad.LoadObject<Sequence<Alias>>("Aliases.xml") ?? new Sequence<Alias>();
			Logger.Success("Псевдонимы загружены");
		}

		public void Save()
		{
			Logger.Log("Сохранение списка псевдонимов...");
			SaveLoad.SaveObject<Sequence<Alias>>(Aliases, "Aliases.xml");
			Logger.Success("Сохранение завершено");
		}

		private static void StaticSave()
		{
			new AliasesController().Save();
		}

		/// <summary>
		/// Создает псевдоним ДЛЯ УЧАСТНИКА
		/// </summary>
		/// <param name="name">Псевдоним</param>
		/// <param name="target">Ссылка на объект Member, которому он принадлежит</param>
		public static Alias AddAlias(String name, Member target)
		{
			Alias alias = new Alias(name, target.Id);
			Aliases.Add(alias);
			StaticSave();
			return alias;
		}

		/// <summary>
		/// Создает псевдоним ДЛЯ УЧАСТНИКА
		/// </summary>
		/// <param name="name">Псевдоним</param>
		/// <param name="target">Id участника.</param>
		public static void AddAlias(String name, ulong target)
		{
			Aliases.Add(new Alias(name, target));
			StaticSave();
		}

		/// <summary>
		/// Удаляет первую в списке связку псевдоним-участник
		/// </summary>
		/// <param name="name">Удаляемый псевдоним</param>
		public static Alias RemoveAlias(String name)
		{
			Alias alias = FindAlias(name);
			Aliases.Remove(alias);
			StaticSave();
			return alias;
		}

		/// <summary>
		/// Удаляет все псевдонимы, связанные с указанным ID
		/// </summary>
		/// <param name="id">Id искомого участника</param>
		public static void RemoveAliases(ulong id)
		{
			foreach (Alias alias in FindAliases(id))
			{
				Aliases.Remove(alias);
			}
			StaticSave();
		}

		/// <summary>
		/// Возвращает пару псевдоним-ID по псевдониму
		/// </summary>
		/// <param name="name">искомый псевдоним</param>
		/// <returns>Объект класса Alias(псевдоним-Id), либо пустой объект класса Alias</returns>
		public static Alias FindAlias(String name)
		{
			foreach (Alias alias in Aliases)
			{
				if (alias.Name.ToLower() == name.ToLower())
				{
					return alias;
				}
			}
			return new Alias();
		}

		/// <summary>
		/// Возвращает список псевдонгимов, соответствующих указанному Id
		/// </summary>
		/// <param name="id">Искомый ID</param>
		/// <returns>Список псевдонимов для указанного Шв</returns>
		public static List<Alias> FindAliases(ulong id)
		{
			List<Alias> aliases = new List<Alias>();

			foreach (Alias alias in Aliases)
			{
				if (alias.ID == id)
				{
					aliases.Add(alias);
				}
			}
			return aliases;
		}
	}
}
