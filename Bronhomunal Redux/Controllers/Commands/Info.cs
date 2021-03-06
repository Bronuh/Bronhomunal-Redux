﻿using Bronuh.Events;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bronuh.Controllers.Commands
{
	class Info : ICommands
	{
		private bool _cached = false;
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

				List<string> tags = new List<string>();
				Dictionary<string, List<string>> sortedCommands = new Dictionary<string, List<string>>();
				List<string> hide = new List<string>(new[] { "[ДАННЫЕ УДАЛЕНЫ]", "█████████" });

				CommandsController.Commands.ForEach(command =>
				{
					command.Tags.ForEach(tag =>
					{
						if (!tags.Contains(tag))
						{
							tags.Add(tag);
							sortedCommands.Add(tag, new List<string>());
						}
						sortedCommands[tag].Add((!command.OpOnly || m.Author.IsOp()) ? command.Name : hide.GetRandom());
					});
				});


				foreach (var kv in sortedCommands)
				{
					respond += $"**{kv.Key}:** ";
					foreach (var cmd in kv.Value)
					{
						respond += $"`{cmd}` ";
					}
					respond += "\n";
				}


				await m.RespondAsync(respond);
			})
			.AddAlias("команды")//.AddAlias("help").AddAlias("хелп").AddAlias("помощь")
			.SetDescription("Выводит список доступных команд")
			.SetUsage("<command> [tag]")
			.AddTag("help")
			.AddTag("info");
			/// ==============================================================================================================

			CommandsController.AddCommand("commandinfo", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				if (parts.Length > 1)
				{
					try
					{
						string respond = CommandsController.FindCommand(parts[1]).GetInfo();
						await m.RespondAsync(respond);
					}
					catch 
					{
						Logger.Warning("Not found: "+parts[1]);
					}
					
				}

			})
			.AddAlias("help").AddAliases("хелп","помощь")
			.SetUsage("<command> команда")
			.SetDescription("Выводит информацию о команде")
			.AddTag("misc")
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
				string serverIp = "abro.cc";
				ushort mainPort = 25567;
				ushort pluginPort = 25564;
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
				if (!_cached)
				{
					int cached = 0;
					AchievementsController.Achievements.EachAsync((a) =>
					{
						cached++;
						a.GetImage();
						Logger.Log($"{cached}) Закэширована ачивка " + a.Name);
					});
					_cached = true;
				}

				await PrintCustomAchievements(m);
				await PrintAchievements(m);
				
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

				string respond = "**Статистика "+target.DisplayName+":**\n";

				respond += target.Statistics.ToString();

				await m.RespondAsync(respond);
			})
			.AddAlias("статы").AddAlias("статистика")
			.SetUsage("<command>")
			.SetDescription("Выводит значения статистики")
			.AddTag("info");
		}



		private static async Task PrintAchievements(ChatMessage m)
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
		}




		private static async Task PrintCustomAchievements(ChatMessage _args)
		{
			var msg = _args;
			string text = _args.Text;
			string[] parts = text.Split(' ');
			int userRank = msg.Author.Rank;

			string args = text.Substring(parts[0].Length);

			Member target = null;

			if (parts.Length > 1)
			{
				target = MembersController.FindMember(parts[1]);
			}
			else if (parts.Length == 1)
			{
				target = msg.Author;
			}


			int images = 0;

			List<CustomAchievement> achs = new List<CustomAchievement>();

			target.CustomValues.ForEach(v => {
				//Logger.Log("checking custom value...");
				//Logger.Log("custom value is "+v.GetValueType());
				if (v.GetValueType() == typeof(CustomAchievement))
				{
					//Logger.Log("custom value is achievement");
					achs.Add((CustomAchievement)v.Value);
				}
			});

			List<CustomAchievement> sorted = new List<CustomAchievement>(achs.OrderByDescending(a => a.Rarity).ThenBy(a => a.Name));

			var messageBuilder = new DiscordMessageBuilder()
			.WithContent(":pencil: Особые достижения пользователя " + target.DisplayName);

			foreach (CustomAchievement achievement in sorted)
			{
				messageBuilder.WithFile(achievement.Name + ".png", achievement.GetImage());
				images++;

				if (images >= 10)
				{
					await msg.RespondAsync(messageBuilder);
					messageBuilder = new DiscordMessageBuilder();
					images = 0;
				}

			}
			await msg.RespondAsync(messageBuilder);
		}
	}
}
