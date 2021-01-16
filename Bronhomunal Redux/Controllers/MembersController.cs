using System;
using System.Collections.Generic;
using System.Text;
using Bronuh.Logic;
using Bronuh.Types;
using DSharpPlus.Entities;
using Bronuh.File;
using System.Threading.Tasks;

namespace Bronuh
{
	
	public static class MembersController
	{
		public static Sequence<Member> Members = new Sequence<Member>();
		


		public static void Load()
		{
			
			Logger.Log("Загрузка списка пользователей...");
			Members = SaveLoad.LoadObject<Sequence<Member>>("Members.xml") ?? new Sequence<Member>();
			Logger.Success("Пользователи загружены");
			
		}

		public static void Save()
		{
			Logger.Log("Сохранение списка пользователей...");
			SaveLoad.SaveObject<Sequence<Member>>(Members, "Members.xml");
			Logger.Success("Сохранение завершено");
		}

		

		


		public static void LinkDiscordMembers(List<DiscordMember> discordMembers)
		{
			foreach (DiscordMember discordMember in discordMembers)
			{
				bool found = false;
				foreach(Member member in Members)
				{
					if (member.Id == discordMember.Id)
					{
						member.Source = discordMember;
						found = true;
						member.Update();
						Logger.Success("Сваязаны: "+member.DisplayName+" -> "+discordMember.DisplayName);
						break;
					}
				}

				if (!found)
				{
					Members.Add(new Member(discordMember));
					Logger.Warning("Не найден: "+discordMember.DisplayName+". Создан новый участник");
				}
			}
		}

		public static void Update()
		{
			Members.Each((m)=> { m.Update(); });
		}

		public static async Task HardUpdate()
		{
			Bot.DiscordMembers = new List<DiscordMember>();
			IReadOnlyCollection<DiscordMember> members = new List<DiscordMember>();
			try
			{
				members = await Bot.Guild.GetAllMembersAsync();
			}
			catch (Exception ex)
			{
				Logger.Error("Дерьмо случается!");
				Logger.Error(ex.Message);
			}
			Logger.Log("Связывание списка участников...");
			foreach (DiscordMember kv in members)
			{
				Bot.DiscordMembers.Add(kv);
			}
			LinkDiscordMembers(Bot.DiscordMembers);
		}

		
		/// <summary>
		/// Эквивалентно FindMemberByID()
		/// </summary>
		/// <param name="ID">ID искомого участника</param>
		/// <returns>Найденный участник, либо null</returns>
		public static Member FindMember(ulong ID)
		{
			return FindMemberByID(ID);
		}


		/// <summary>
		/// Ищет участника по входной строке в следующем порядке:
		///     - По частичному совпадению с ником
		///     - По полному совпадению с псевдонимом
		///     - по точному значению ID
		/// </summary>
		/// <param name="name">Строка, соответствующая искомому участнику (ник/псевдоним/ID)</param>
		/// <returns>Найденный участник, либо null</returns>
		public static Member FindMember(string name)
		{
			Member Found = null;
			Logger.Debug("      Поиск по нику...");
			if (name.Length >= 2)
			{
				Found = FindMemberByNickname(name);
				if (Found == null)
				{
					Logger.Debug("      Поиск по имени...");
					Found = FindMemberByName(name);
				}
				if (Found == null)
				{
					Logger.Debug("      Поиск по псевдонимам...");
					Found = FindMemberByAlias(name);
				}
				if (Found == null)
				{
					ulong id = ulong.Parse(name);
					Logger.Debug("      Поиск по ID (" + id + ")...");
					Found = FindMemberByID(id);
				}
			}

			return Found;
		}


		/// <summary>
		/// Находит участника по частичному совпадению ника. Возвращен будет участник с первым попавшимся совпадением. Список не отсортирован.
		/// </summary>
		/// <param name="name">Ник искомого участника</param>
		/// <returns>Найденный участник, либо null</returns>
		public static Member FindMemberByNickname(string nickname)
		{
			Member Found = null;
			foreach (Member M in Members)
			{
				if (M.Nickname.ToLower().Contains(nickname.ToLower()))
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("Участник не найден. Попытка поиска по базовым спискам");
				DiscordMember m = FindDiscordMemberByNickname(nickname);
				if (m != null)
				{
					Found = new Member(m);
				}
				else
				{
					Logger.Debug("DiscordMember не найден. Возврат null");
				}


			}
			else
			{
				Logger.Debug("Участник найден: " + Found.Nickname + "#" + Found.Discriminator);
				if (Found.Source == null)
				{
					Logger.Debug("Не указана ссылка на объект DiscordMember. Ищем...");
					DiscordUser ptr = FindDiscordMemberByNickname(nickname);
					if (ptr == null && Found != null)
					{
						Logger.Debug("Объект DiscordMember не найден. Удаление участника...");
						Members.Remove(Found);
						Found = null;
					}
				}
			}

			return Found;
		}

		/// <summary>
		/// Находит участника по ID. Если нет участника Member, но есть DiscordMember, то создаст нового участника.
		/// </summary>
		/// <param name="ID">ID искомого участника</param>
		/// <returns>Найденный участник, либо null</returns>
		public static Member FindMemberByID(ulong ID)
		{
			Member Found = null;
			foreach (Member M in Members)
			{
				if (M.Id == ID)
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("Участник не найден. Попытка поиска по базовым спискам");
				DiscordMember m = FindDiscordMemberByID(ID);
				if (m != null)
				{
					Found = new Member(m);
				}
				else
				{
					Logger.Debug("DiscordMember не найден. Возврат null");
				}
			}
			else
			{
				Logger.Debug("Участник найден: " + Found.Username + "#" + Found.Discriminator);
				if (Found.Source == null)
				{
					Logger.Debug("Не указана ссылка на объект DiscordMember. Ищем...");
					DiscordMember ptr = FindDiscordMemberByID(ID);
					if (ptr == null && Found != null)
					{
						Logger.Debug("Объект DiscordMember не найден. Удаление участника...");
						Members.Remove(Found);
						Found = null;
					}
					else
					{
						Found.Source = ptr;
					}
				}
			}

			return Found;
		}

		/// <summary>
		/// Находит DiscordMember в общем списке по частичному совпадению никнейма. Возвращает первое совпадение в неотсортированном списке.
		/// </summary>
		/// <param name="name">Искомый ник</param>
		/// <returns>DiscordMember либо null</returns>
		public static DiscordMember FindDiscordMemberByNickname(string nickname)
		{
			DiscordMember Found = null;

			foreach (DiscordMember M in Bot.DiscordMembers)
			{
				if (M.DisplayName.ToLower().Contains(nickname.ToLower()))
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("В базовых списках участника с таким Ником нет");
			}
			else
			{
				Logger.Debug("Участник (DiscordMember) найден: " + Found.DisplayName + "#" + Found.Discriminator);
			}

			return Found;
		}

		/// <summary>
		/// Находит участника по частичному совпадению ника. Возвращен будет участник с первым попавшимся совпадением. Список не отсортирован.
		/// </summary>
		/// <param name="name">Ник искомого участника</param>
		/// <returns>Найденный участник, либо null</returns>
		public static Member FindMemberByName(string name)
		{
			Member Found = null;
			foreach (Member M in Members)
			{
				if (M.Username.ToLower().Contains(name.ToLower()))
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("Участник не найден. Попытка поиска по базовым спискам");
				DiscordMember m = FindDiscordMemberByName(name);
				if (m != null)
				{
					Found = new Member(m);
				}
				else
				{
					Logger.Debug("DiscordMember не найден. Возврат null");
				}


			}
			else
			{
				Logger.Debug("Участник найден: " + Found.Username + "#" + Found.Discriminator);
				if (Found.Source == null)
				{
					Logger.Debug("Не указана ссылка на объект DiscordMember. Ищем...");
					DiscordUser ptr = FindDiscordMemberByName(name);
					if (ptr == null && Found != null)
					{
						Logger.Debug("Объект DiscordMember не найден. Удаление участника...");
						Members.Remove(Found);
						Found = null;
					}
				}
			}

			return Found;
		}

		/// <summary>
		/// Находит DiscordMember в общем списке по Id
		/// </summary>
		/// <param name="ID">Искомый Id</param>
		/// <returns>DiscordMember либо null</returns>
		public static DiscordMember FindDiscordMemberByID(ulong ID)
		{
			DiscordMember Found = null;

			foreach (DiscordMember M in Bot.DiscordMembers)
			{
				if (M.Id == ID)
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("В базовых списках участника с таким Id нет");
			}
			else
			{
				Logger.Debug("Участник (DiscordMember) найден: " + Found.Username + "#" + Found.Discriminator);
			}

			return Found;
		}

		/// <summary>
		/// Находит DiscordMember в общем списке по частичному совпадению ника. Возвращает первое совпадение в неотсортированном списке.
		/// </summary>
		/// <param name="name">Искомый ник</param>
		/// <returns>DiscordMember либо null</returns>
		public static DiscordMember FindDiscordMemberByName(string name)
		{
			DiscordMember Found = null;

			foreach (DiscordMember M in Bot.DiscordMembers)
			{
				if (M.Username.ToLower().Contains(name.ToLower()))
				{
					Found = M;
					break;
				}
			}

			if (Found == null)
			{
				Logger.Debug("В базовых списках участника с таким Ником нет");
			}
			else
			{
				Logger.Debug("Участник (DiscordMember) найден: " + Found.Username + "#" + Found.Discriminator);
			}

			return Found;
		}

		/// <summary>
		/// Находит участнгика по точному совпадению псевдонима
		/// </summary>
		/// <param name="name">Искомый псевдоним</param>
		/// <returns>Искомый участник либо null</returns>
		public static Member FindMemberByAlias(String name)
		{
			return FindMemberByID(AliasesController.FindAlias(name).ID);
		}



		
	}
}
