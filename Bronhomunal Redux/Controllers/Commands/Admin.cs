using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Text;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;

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
			}).SetOp(true).AddTag("admin");


			CommandsController.AddCommand("say", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;


				string respond = text.Replace(parts[0] + " ", "");
				// Program.Server.PushMessage(respond);
				await m.RespondAsync(respond);
				await m.Source.DeleteAsync();
			})
			.AddAlias("скажи")
			.SetOp(false)
			.SetDescription("Заставляет бота сказать что-то")
			.AddTag("misc")
			.AddTag("fun");



			CommandsController.AddCommand("test", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				Bitmap baseBitmap = Bronuh.Properties.Resources.Level;
				Bitmap avatarBitmap = m.Author.GetAvatar();

				var baseImage = Image.Load(baseBitmap.ToArray());
				var avatarImage = Image.Load(avatarBitmap.ToArray());

				avatarImage.Mutate(x => x.Resize(new ResizeOptions { 
					Size = new SixLabors.ImageSharp.Size(110,110),
					Mode = ResizeMode.Crop
				}).ApplyRoundedCorners(10));

				baseImage.Mutate(ctx => {
					int step = (128 - 110) / 2;
					ctx.DrawImage(avatarImage, new SixLabors.ImageSharp.Point(720-110-step,step), 1);
					ctx.DrawText(new TextGraphicsOptions { 
						TextOptions = { 
							HorizontalAlignment = HorizontalAlignment.Center,
						}
					},
					m.Author.Rank+"",
					SixLabors.Fonts.SystemFonts.CreateFont("Arial", 60),
					new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
					new SixLabors.ImageSharp.PointF(68, 30));

					var col = m.Author.Source.Color;

					ctx.DrawText(new TextGraphicsOptions
					{
						TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
						}
					},
					m.Author.Username,
					SixLabors.Fonts.SystemFonts.CreateFont("Arial", 50),
					new SixLabors.ImageSharp.Color(new Rgba32(col.R, col.G, col.B)),
					new SixLabors.ImageSharp.PointF(150, 33));

				});

				var bitmap = m.Author.GetAvatar();
				MemoryStream memoryStream = new MemoryStream();
				baseImage.SaveAsPng(memoryStream);
				//bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				memoryStream.Position = 0;

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithContent("Ок, тест")
					.WithFile("Test.png", memoryStream));
			})
			.AddAlias("тест")
			.SetDescription("Делает какую-то произвольную хардкодную дичь")
			.SetOp(false)
			.AddTag("admin")
			.AddTag("test");


			CommandsController.AddCommand("shutdown", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOp())
				{
					Program.SaveAll();
					System.Diagnostics.Process.Start("cmd", "/c shutdown /s /t 0");
				}
			}).SetOp(true)
			.AddTag("admin");


			CommandsController.AddCommand("kill", async e =>
			{
				Member sender = e.Author;
				if (sender.IsOp())
				{
					Program.Server.PushMessage("Shutdown");
					await e.RespondAsync("DED");
					Program.SaveAll();
					
					Environment.Exit(0);
				}
			}).SetOp(true).AddAlias("умри").AddAlias("die")
			.AddTag("admin");


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
			}).SetOp(true)
			.AddTag("admin");


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
		}


		
	}
}
