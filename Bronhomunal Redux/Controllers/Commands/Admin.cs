using Bronuh.Graphics;
using Bronuh.Logic;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;

namespace Bronuh.Controllers.Commands
{
	// TODO: асинхронная регенерация ачивок
	class Admin : ICommands
	{
		public void InitializeCommands()
		{
			/// ============================================================================================================================
			CommandsController.AddCommand("debug", async (m) =>
			{
				Settings.DEBUG = !Settings.DEBUG;

				string respond = "Отображение сообщений отладки: " + Settings.DEBUG;

				await m.RespondAsync(respond);
			})
			.SetOp(true)
			.SetDescription("Включает/выключает вывод отладочных сообщений в консоль")
			.AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("regenerate", async (m) =>
			{
				await m.RespondAsync("Кэширование ачивок...");
				int cached = 0;
				foreach (var ach in AchievementsController.Achievements)
				{
					cached++;
					ach.GetImage();
					Logger.Log($"{cached}) Закэширована ачивка "+ach.Name);
				}

				string respond = $"Закэшгировано {cached} ачивок";

				await m.RespondAsync(respond);
			})
			.SetOp(true)
			.SetDescription("Заново кэширует данные")
			.AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("token", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string token = parts[1];
				if (m.Author.IsOwner())
				{
					Settings.SetToken(token);

					await CommandsController.TryExecuteConsoleCommand("kill");

					string respond = "Установлен токен: " + token;
					await m.RespondAsync(respond);
				}
				
			}).SetOp(true).AddTags("admin", "owner"); ;

			/// ============================================================================================================================
			CommandsController.AddCommand("say", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				string respond = text.Replace(parts[0] + " ", "");
				await m.RespondAsync(respond);
				await m.Source?.DeleteAsync();
				await m.Author.GiveAchievement("itsnotme");
				if (respond.StartsWith(Program.Prefix+Settings.Sign))
				{
					await m.Author.GiveAchievement("tricky");
				}
			})
			.AddAlias("скажи")
			.SetOp(false)
			.SetDescription("Заставляет бота сказать что-то")
			.AddTag("misc")
			.AddTag("fun");

			/// ============================================================================================================================
			CommandsController.AddCommand("test", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;
				
			})
			.AddAlias("тест")
			.SetDescription("Делает какую-то произвольную хардкодную дичь")
			.SetOp(true)
			.AddTag("admin")
			.AddTag("test");

			/// ============================================================================================================================
			CommandsController.AddCommand("shutdown", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOwner())
				{
					Program.SaveAll();
					System.Diagnostics.Process.Start("cmd", "/c shutdown /s /t 0");
				}
			}).SetOp(true)
			.AddTags("admin", "owner");

			/// ============================================================================================================================
			CommandsController.AddCommand("kill", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOwner())
				{
					Program.Server.PushMessage("Shutdown");
					await e.RespondAsync("DED");
					Program.SaveAll();

					Environment.Exit(0);
				}
			}).SetOp(true).AddAlias("умри").AddAlias("die")
			.AddTags("admin","owner");

			/// ============================================================================================================================
			CommandsController.AddCommand("restart", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOp())
				{
					Program.Server.PushMessage("Restart");
					await e.RespondAsync("DED");
					Program.SaveAll();

					Environment.Exit(0);
				}
			}).SetOp(true).AddAlias("перезайди").AddAlias("рестарт")
			.AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("save", async e =>
			{
				Member sender = e.Author;
				await e.RespondAsync("Сохраняю...");
				Program.SaveAll();

			}).SetOp(true).AddAlias("сохранить").AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("op", async e =>
			{
				string[] parts = e.Text.Split(' ');

				if (Bot.Ready)
				{
					try
					{
						Member member = MembersController.FindMember(parts[1]);
						member.IsOP = true;
						Logger.Success("Выданы права администратора пользователю " + member.DisplayName);
					}
					catch (Exception ex)
					{
						Logger.Error(ex.Message);
					}
				}
				else
				{
					Logger.Warning("Бот еще не готов");
				}
			}).SetOp(true)
			.AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("deop", async e =>
			{
				string[] parts = e.Text.Split(' ');

				if (Bot.Ready)
				{
					try
					{
						Member member = MembersController.FindMember(parts[1]);
						member.IsOP = false;
						Logger.Success("Пользователь лишен прав администратора" + member.DisplayName);
					}
					catch (Exception ex)
					{
						Logger.Error(ex.Message);
					}
				}
				else
				{
					Logger.Warning("Бот еще не готов");
				}
			}).SetOp(true)
			.AddTag("admin");

			/// ============================================================================================================================
			CommandsController.AddCommand("giveachievement", async (message) =>
			{
				string text = message.Text;
				string[] parts = text.Split(' ');
				string username = parts[1];

				var target = MembersController.FindMember(username);
				if (target == null)
				{
					return;
				}
				if (parts.Length == 3)
				{
					await target.GiveAchievement(parts[2]);
				}
				if (parts.Length == 6)
				{
					var name = parts[2].Replace("_", " ");
					var description = parts[3].Replace("_", " ");
					var rarity = (Rarity)Enum.Parse(typeof(Rarity), parts[4].ToUpper());
					var link = parts[5];

					target.GiveCustomAchievement(new CustomAchievement()
					.SetName(name)
					.SetDescription(description)
					.SetRarity(rarity)
					.SetImage(link),
					parts[2].ToLower());
				}
			})
			.SetOp(true)
			.SetUsage("<command> пользователь ")
			.AddTag("admin");
		}
	}
}
