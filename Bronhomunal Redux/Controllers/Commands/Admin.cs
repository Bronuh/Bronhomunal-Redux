using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Text;

namespace Bronuh.Controllers.Commands
{
	class Admin : ICommands
	{
		public void InitializeCommands()
		{
			CommandsController.AddCommand("debug", async (m) =>
			{
				Settings.DEBUG = !Settings.DEBUG;

				string respond = "Отображение сообщений отладки: " + Settings.DEBUG;

				await m.RespondAsync(respond);
			})
			.SetOp(true)
			.SetDescription("Включает/выключает вывод отладочных сообщений в консоль")
			.SetUsage(Settings.Sign + "debug")
			.AddTag("admin");


			CommandsController.AddCommand("token", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string token = parts[1];
				Settings.SetToken(token);

				await CommandsController.TryExecuteConsoleCommand("kill");

				string respond = "Установлен токен: " + token;
				await m.RespondAsync(respond);
			}).AddTag("admin");


			CommandsController.AddCommand("say", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;


				string respond = text.Replace(parts[0] + " ", "");
				Program.Server.PushMessage(respond);
				await m.RespondAsync(respond);
			})
			.AddAlias("скажи")
			.SetOp(true)
			.SetDescription("Заставляет бота сказать что-то")
			.AddTag("admin");



			CommandsController.AddCommand("test", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;


				var bitmap = Bronuh.Properties.Resources.Test;
				MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				memoryStream.Position = 0;

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithContent("Ок, тест")
					.WithEmbed(new DiscordEmbedBuilder()
						.WithImageUrl("https://puu.sh/H8LYq.png")
						.Build())
					.WithFile("Test.jpg",memoryStream));
			})
			.AddAlias("тест")
			.SetOp(true).AddTag("admin").AddTag("test");


			CommandsController.AddCommand("shutdown", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOP)
				{
					Program.SaveAll();
					System.Diagnostics.Process.Start("cmd", "/c shutdown /s /t 0");
				}
			}).SetOp(true).AddTag("admin");


			CommandsController.AddCommand("kill", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOP)
				{
					Program.Server.PushMessage("Shutdown");
					await e.RespondAsync("DED");
					Program.SaveAll();
					
					Environment.Exit(0);
				}
			}).SetOp(true).AddAlias("умри").AddAlias("die").AddTag("admin");


			CommandsController.AddCommand("restart", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOP)
				{
					Program.Server.PushMessage("Restart");
					await e.RespondAsync("DED");
					Program.SaveAll();

					Environment.Exit(0);
				}
			}).SetOp(true).AddAlias("перезайди").AddAlias("рестарт").AddTag("admin");


			CommandsController.AddCommand("save", async e =>
			{
				Member sender = e.Author;
				await e.RespondAsync("Сохраняю...");
				Program.SaveAll();

			}).SetOp(true).AddAlias("сохранить").AddTag("admin");

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
			}).SetOp(true).AddTag("admin");


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
			}).SetOp(true).AddTag("admin");
		}
	}
}
