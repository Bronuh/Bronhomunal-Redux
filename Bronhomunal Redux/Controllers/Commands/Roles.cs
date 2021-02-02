using Bronuh.Types;
using DSharpPlus.Entities;


namespace Bronuh.Controllers.Commands
{
	class Roles : ICommands
	{
		private static readonly string[] _restricted = new[] {
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
			.SetDescription("Выводит список ролей, доступных для использования")
			.AddTag("info");

			CommandsController.AddCommand("giverole", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;
				string other = "";

				for (int i = 1; i < parts.Length; i++)
				{
					other += parts[i];
					if (i != parts.Length - 1)
					{
						other += " ";
					}
				}

				DiscordRole foundRole = null;

				if (!user.IsConsole())
				{
					foreach (DiscordRole role in Bot.Guild.Roles.Values)
					{
						if (role.Name.ToLower().Contains(other.ToLower()))
						{
							Logger.Success("Найдена роль: " + role.Name);
							foundRole = role;
							break;
						}
					}
				}

				if (CheckRole(foundRole, user.Source))
				{
					Logger.Debug("Проверки пройдены, выдача роли...");
					await user.Source.GrantRoleAsync(foundRole, "По запросу");
					Logger.Debug("роль выдана");
					string respond = "Выдана роль: " + foundRole.Name;
					await m.RespondAsync(respond);
					await MembersController.HardUpdate();
				}

			})
			.SetDescription("Выдает пользователю роль")
			.SetUsage("<command> название_роли")
			.AddTag("misc");

			CommandsController.AddCommand("takerole", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;
				string other = "";

				for (int i = 1; i < parts.Length; i++)
				{
					other += parts[i];
					if (i != parts.Length - 1)
					{
						other += " ";
					}
				}

				DiscordRole foundRole = null;

				if (!user.IsConsole())
				{
					foreach (DiscordRole role in Bot.Guild.Roles.Values)
					{
						if (role.Name.ToLower().Contains(other.ToLower()))
						{
							Logger.Success("Найдена роль: " + role.Name);
							foundRole = role;
							break;
						}
					}
				}

				if (user.HasRole(foundRole))
				{
					Logger.Debug("Проверки пройдены, отмена роли...");
					await user.Source.RevokeRoleAsync(foundRole, "По запросу");
					Logger.Debug("роль отменена");
					string respond = "Отменена роль: " + foundRole.Name;
					await m.RespondAsync(respond);
					await MembersController.HardUpdate();
				}

			})
			.SetDescription("Отменяет роль пользователя")
			.SetUsage("<command> название_роли")
			.AddTag("misc");

			CommandsController.AddCommand("createrole", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;
				string other = "";

				for (int i = 1; i < parts.Length; i++)
				{
					other += parts[i];
					if (i != parts.Length - 1)
					{
						other += " ";
					}
				}

				if (!Exists(other))
				{
					await Bot.Guild.CreateRoleAsync(other, mentionable: true);
				}

			})
			.SetDescription("Создает новую роль")
			.SetUsage("<command> название_роли")
			.SetRank(2)
			.AddTag("misc");
		}

		private static bool CheckRole(DiscordRole role, DiscordMember? member)
		{
			Logger.Debug("CheckRole started with '" + role.Name + "'");
			if (role.Permissions != DSharpPlus.Permissions.All)
			{
				foreach (string name in _restricted)
				{
					if (name.ToLower() == role.Name.ToLower())
					{
						Logger.Warning("Запрещенная роль: " + name);
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
				Logger.Debug("Допустимая роль: " + role.Name);
				return true;
			}
			return false;
		}

		private static bool Exists(string roleName)
		{
			foreach (DiscordRole role in Bot.Guild.Roles.Values)
			{
				if (role.Name.ToLower() == roleName.ToLower())
				{
					return true;
				}
			}
			return false;
		}
	}
}
