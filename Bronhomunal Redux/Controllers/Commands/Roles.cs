using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Bronuh.Controllers.Commands
{
	class Roles : ICommands
	{
		private static string[] _restricted = new [] { 
			"@everyone",
			"Vaster Lord",
			"OverLord",
			"Вжухушек",
			"Bot",
			"Staff",
			"Tatsumaki",
			"Bongo",
			"Bronhomunal",
			"Бронхи",
			"Всратый Лорд",
			"Hydra"
		};
		public void InitializeCommands()
		{
			CommandsController.AddCommand("roles", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				var roles = Bot.Guild.Roles.Values;


				string respond = "Роли: \n";

				foreach (DiscordRole role in roles)
				{
					if (CheckRole(role, m.Author.Source))
					{
						respond += role.Name + "\n";
					}
				}
				await m.RespondAsync(respond);
			})
			.AddAlias("роли")
			.SetDescription("Выводит список ролей, доступных для использования");


			//TODO: разобраться почему нельзя выдать роли, написанные латиницей
			CommandsController.AddCommand("addrole", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;

				DiscordRole foundRole = null;

				if (!user.IsConsole())
				{
					foreach (DiscordRole role in Bot.Guild.Roles.Values)
					{
						if (role.Name.ToLower() == parts[1].ToLower())
						{
							Logger.Success("Найдена роль: "+role.Name);
							foundRole = role;
							break;
						}
					}
				}

				if (CheckRole(foundRole, user.Source))
				{
					Logger.Debug("Проверки пройдены, выдача роли...");
					await user.Source.GrantRoleAsync(foundRole);
					Logger.Debug("роль выдана");
					string respond = "Выдана роль: " + foundRole.Name;
					await m.RespondAsync(respond);
					await MembersController.HardUpdate();
				}

			})
			.SetDescription("Выдает пользователю роль")
			.SetUsage(Settings.Sign+"addrole название_роли");
		}


		private static bool CheckRole(DiscordRole role)
		{
			return CheckRole(role, null);
		}



		private static bool CheckRole(DiscordRole role, DiscordMember? member)
		{
			Logger.Debug("CheckRole started with '"+role.Name+"'");
			if (role.Permissions != DSharpPlus.Permissions.All)
			{

				foreach (string name in _restricted)
				{
					if (name.ToLower() == role.Name.ToLower()) 
					{
						Logger.Warning("Запрещенная роль: "+name);
						return false; 
					}
				}

				if (member != null)
				{
					if (member.HasRole(role))
					{
						Logger.Warning("Пользователь уже имеет роль: " + role.Name);
						return false;
					}
				}
				Logger.Debug("Допустимая роль: "+role.Name);
				return true;
			}
			return false;
		}
	}
}
