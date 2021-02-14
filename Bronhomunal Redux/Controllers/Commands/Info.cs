using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bronuh.Controllers.Commands
{
	class Info : ICommands
	{
		public void InitializeCommands()
		{
			/// ==============================================================================================================
			CommandsController.AddCommand("commands", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				int maxCommandsPerMessage = 5;
				int cmdInMessage = 0;

				string respond = "Список команд: \n\n";
				if (parts.Length > 1)
				{
					respond = "Список команд с тегом " + parts[1] + ": \n\n";
					if (parts[1].ToLower()=="admin")
					{
						await m.Author.GiveAchievement("curious");
					}
				}

				foreach (Command command in CommandsController.Commands)
				{
					Logger.Debug("Команда " + command.Name);
					if (userRank >= command.Rank)
					{
						if (command.OpOnly && !m.Author.IsOp())
						{
							continue;
						}
						if (cmdInMessage >= maxCommandsPerMessage)
						{
							Logger.Debug("Слишком много команд. Новое сообщение...");
							await m.RespondPersonalAsync(respond);
							cmdInMessage = 0;
							respond = " \n\n";
						}
						if (parts.Length > 1)
						{
							Logger.Debug("Поиск по тегу " + parts[1] + " среди тегов " + command.Tags.ToLine());
							
							if (command.HasTag(parts[1]))
							{
								Logger.Debug("Тег " + parts[1] + " имется");
								respond += command.GetInfo() + "\n\n";
								cmdInMessage++;
							}
						}
						else
						{
							respond += command.GetInfo() + "\n\n";
							cmdInMessage++;
						}
					}
				}

				await m.RespondPersonalAsync(respond);
			})
			.AddAlias("команды").AddAlias("help").AddAlias("хелп").AddAlias("помощь")
			.SetDescription("Выводит список доступных команд")
			.SetUsage("<command> [tag]")
			.AddTag("help")
			.AddTag("info");

			/// ==============================================================================================================
			CommandsController.AddCommand("whois", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = null;

				if (parts.Length > 1)
				{
					target = MembersController.FindMember(parts[1]);
					m.Author.Statistics.WhoisOther++;
				}
				else if (parts.Length == 1)
				{
					target = m.Author;
					m.Author.Statistics.WhoisSelf++;
					if (m.Author.Statistics.WhoisSelf >= AchievementsController.WhoAmI.CustomValue)
					{
						await m.Author.GiveAchievement("whoami");
					}
				}
				m.Author.Statistics.WhoisTotal++;
				if (m.Author.Statistics.WhoisTotal >= AchievementsController.Major.CustomValue)
				{
					await m.Author.GiveAchievement("major");
				}
				string respond = target?.GetInfo() ?? "";
				await m.RespondAsync(respond);
			})
			.AddAlias("who").AddAlias("кто")
			.SetUsage("<command> [username]")
			.SetDescription("Выводит информацию о пользователе")
			.AddTag("misc")
			.AddTag("info");

			/// ==============================================================================================================
			CommandsController.AddCommand("profile", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = null;

				if (parts.Length > 1)
				{
					target = MembersController.FindMember(parts[1]);
				}
				else if (parts.Length == 1)
				{
					target = m.Author;
				}

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithContent(":pencil: Профиль пользователя " + target.DisplayName)
					.WithFile(target.DisplayName + ".png", target.GetBasicProfileImageStream()));
			})
			.AddAlias("профиль")
			.SetUsage("<command> [username]")
			.SetDescription("Выводит информацию о пользователе")
			.AddTag("misc")
			.AddTag("info")
			.AddTag("test");

			/// ==============================================================================================================
			CommandsController.AddCommand("about", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				string args = text.Substring(parts[0].Length);

				m.Author.About = args;
				await m.RespondAsync("Информация изменена");
				await m.Author.GiveAchievement("about");
			})
			.AddAlias("осебе")
			.SetUsage("<command> текст описания")
			.SetDescription("Изменяет информацию о себе")
			.AddTag("misc")
			.AddTag("info");

			/// ==============================================================================================================
			CommandsController.AddCommand("server", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				string serverIp = "abro.tech";
				ushort mainPort = 25565;
				ushort pluginPort = 25566;
				MineStat ms = new MineStat(serverIp, mainPort);

				string args = text.Substring(parts[0].Length);
				string respond = "**Статус сервера: **\n";
				if (ms.ServerUp)
				{
					respond += ":white_check_mark: ВКЛЮЧЕН\n";
					respond += "Версия: " + ms.Version + "\n";
					respond += "Игроков: " + ms.CurrentPlayers + "/" + ms.MaximumPlayers + "\n";
					respond += "MOTD: " + ms.Motd+"\n";
					try
					{
						respond += "Список игроков: " + AbroServer.Request(serverIp, pluginPort, "@playerslist").Trim()
							.Replace("@text//", "");
					}
					catch (Exception e)
					{
						Logger.Warning(e.Message);
						respond += "Список игроков не получен";
					}
				}
				else
				{
					respond += ":no_entry: ВЫКЛЮЧЕН\n";
				}

				await m.RespondAsync(respond);
			})
			.AddAlias("сервер")
			.SetDescription("Проверяет статус Minecraft сервера")
			.AddTag("misc")
			.AddTag("info");

			/// ==============================================================================================================
			CommandsController.AddCommand("achievements", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				string args = text.Substring(parts[0].Length);

				Member target = null;

				if (parts.Length > 1)
				{
					target = MembersController.FindMember(parts[1]);
				}
				else if (parts.Length == 1)
				{
					target = m.Author;
				}

				if (target != m.Author)
				{
					await m.Author.GiveAchievement("others");
				}

				int images = 0;

				List<Achievement> achs = new List<Achievement>();

				foreach (string achivementId in target.Achievements)
				{
					var achievement = AchievementsController.Find(achivementId);
					if (achievement != null)
					{
						achs.Add(achievement);
					}
				}

				List<Achievement> sorted = new List<Achievement>(achs.OrderByDescending(a => a.Rarity).ThenBy(a => a.Name));

				var messageBuilder = new DiscordMessageBuilder()
				.WithContent(":pencil: Достижения пользователя " + target.DisplayName + $"[{sorted.Count}/{AchievementsController.Achievements.Count}]");

				foreach (Achievement achievement in sorted)
				{
					messageBuilder.WithFile(achievement.Name + ".png", achievement.GetImage());
					images++;

					if (images >= 10)
					{
						await m.RespondAsync(messageBuilder);
						messageBuilder = new DiscordMessageBuilder();
						images = 0;
					}

				}
				await m.RespondAsync(messageBuilder);
			})
			.AddAlias("ачивки").AddAlias("достижения")
			.SetUsage("<command> [username]")
			.SetDescription("Показывает достижения указанного пользователя")
			.AddTag("misc")
			.AddTag("info")
			.AddTag("fun");

			/// ==============================================================================================================
			CommandsController.AddCommand("allachievements", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				string args = text.Substring(parts[0].Length);

				List<Achievement> achs = new List<Achievement>();

				List<Achievement> sorted = new List<Achievement>(AchievementsController.Achievements
					.OrderBy(a => a.Name));

				string respond = ":pencil: Список достижений: \n\n";

				int achievs = 0;

				foreach (Achievement achievement in sorted)
				{
					string has = ":red_circle:";
					if (m.Author.HasAchievement(achievement))
					{
						has = ":green_circle:";
					}
					respond += $"**{has}{achievement.Name}**\n" +
					$"{achievement.GetDescription()}\n\n";
					achievs++;

					if (achievs >= 10)
					{
						await m.RespondAsync(respond);
						achievs = 0;
						respond = "\n\n";
					}
				}

				await m.RespondAsync(respond);
			})
			.AddAlias("всеачивки").AddAlias("вседостижения")
			.SetUsage("<command>")
			.SetDescription("Показывает все существующие достижения")
			.AddTag("misc")
			.AddTag("info")
			.AddTag("fun");

			/// ==============================================================================================================
			CommandsController.AddCommand("commandtags", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				List<string> tags = new List<string>();
				CommandsController.Commands.ForEach(command =>
				{
					command.Tags.ForEach(tag =>
					{
						if (!tags.Contains(tag))
							tags.Add(tag);
					});
				});

				string respond = "Тэги команд:\n";
				foreach (string tag in tags)
					respond += tag + (tag == tags[tags.Count-1] ? "" : ", ");

				await m.RespondAsync(respond);
			})
			.AddAlias("tags").AddAlias("тэги")
			.SetUsage("<command>")
			.SetDescription("Выводит известные теги для команд, для использования с !commands [tag]")
			.AddTag("help")
			.AddTag("info");

			/// ==============================================================================================================
			CommandsController.AddCommand("stats", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = null;

				if (parts.Length > 1)
				{
					target = MembersController.FindMember(parts[1]);
				}
				else if (parts.Length == 1)
				{
					target = m.Author;
				}

				if (target==null)
				{
					await m.RespondAsync("Пользователь не найден");
				}

				string respond = "**Статистика:**\n";

				var fields = typeof(MemberStatistics).GetFields();
				foreach (var field in fields)
				{
					string fieldAbout = "";
					object[] attrs = field.GetCustomAttributes(false);
					foreach (AboutAttribute attr in attrs)
					{
						fieldAbout = attr.About;
						break;
					}
					var value = field.GetValue(target.Statistics);
					respond += fieldAbout + ": " + value+"\n";
				}

				await m.RespondAsync(respond);
			})
			.AddAlias("статы").AddAlias("статистика")
			.SetUsage("<command>")
			.SetDescription("Выводит значения статистики")
			.AddTag("info");
		}
	}
}
