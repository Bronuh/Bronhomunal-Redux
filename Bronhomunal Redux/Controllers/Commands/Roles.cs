using Bronuh.Types;
using DSharpPlus.Entities;
using System;

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
			/// ============================================================================================================================
			CommandsController.AddCommand("roles", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				var roles = Bot.Guild.Roles.Values;

				string respond = "Доступные для выдачи роли: \n";

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

			/// ============================================================================================================================
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

				if (foundRole == null)
				{
					await m.RespondAsync("Роль не найдена");
				}

				if (CheckRole(foundRole, user.Source))
				{
					Logger.Debug("Проверки пройдены, выдача роли...");
					await user.Source.GrantRoleAsync(foundRole, "По запросу");
					Logger.Debug("роль выдана");
					string respond = "Выдана роль: " + foundRole.Name;
					await m.RespondAsync(respond);
					await MembersController.HardUpdate();
					await user.GiveAchievement("fitting");
				}

			})
			.SetDescription("Выдает вызвавшему пользователю указанную роль")
			.SetUsage("<command> название_роли")
			.AddTag("misc");

			/// ============================================================================================================================
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

				if (foundRole == null)
				{
					await m.RespondAsync("Роль не найдена");
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
			.SetDescription("Отменяет роль вызвавщему пользователю")
			.SetUsage("<command> название_роли")
			.AddTag("misc");

			/// ============================================================================================================================
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
					var role = await Bot.Guild.CreateRoleAsync(other, mentionable: true);
					await user.Source.GrantRoleAsync(role, "По запросу");
					string respond = "Создана и выдана роль: " + role.Name;
					await m.RespondAsync(respond);
					await MembersController.HardUpdate();
					await user.GiveAchievement("diy");
				}

			})
			.SetDescription("Создает новую роль и выдает её пользователю")
			.SetUsage("<command> название_роли")
			.SetRank(4)
			.AddTag("misc");

			/// ============================================================================================================================
			CommandsController.AddCommand("rolecolor", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;
				byte r, g, b;
				try
				{
					r = Byte.Parse(parts[2]);
					g = Byte.Parse(parts[3]);
					b = Byte.Parse(parts[4]);
				}
				catch (Exception e)
				{
					Logger.Error(e.Message);
					return;
				}

				if (parts.Length == 5) {
					if (Exists(parts[1]))
					{
						DiscordRole foundDole = null;
						foreach (DiscordRole role in Bot.Guild.Roles.Values)
						{
							if (role.Name.ToLower() == parts[1].ToLower())
							{
								await role.ModifyAsync(color: new DiscordColor(r,g,b));
								string respond = "Изменяется цвет роли: " + role.Name;
								await user.GiveAchievement("colored");
								await m.RespondAsync(respond);
							}
						}
					}
				}

			})
			.SetDescription("Меняет цвет роли")
			.SetUsage("<command> название_роли R G B\n\t или <command> название_роли цветовой_код")
			.SetRank(4)
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
