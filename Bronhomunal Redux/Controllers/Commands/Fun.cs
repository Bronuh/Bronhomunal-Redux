using Bronuh.Libs;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.IO;

namespace Bronuh.Controllers.Commands
{
	class Fun : ICommands
	{
		public void InitializeCommands()
		{


			CommandsController.AddCommand("waste", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				string respond = Wastificator.Wastificate(text.Replace(parts[0] + " ", ""));
				await m.RespondAsync(respond);
				await m.Author.GiveAchievement("wasted");
			})
			.AddAlias("потратить").AddAlias("потрачено")
			.SetDescription("Заставляет бота потратить что-то")
			.SetUsage("<command> текст который должен быть портачен")
			.AddTag("fun");


			CommandsController.AddCommand("infa", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				string respond = args + "\nИнфа: " + Math.Round(Infa.CheckInfo(args).Value, 0) + "%";
				await m.RespondAsync(respond);
				await m.Author.GiveAchievement("infameter");
			})
			.AddAlias("info").AddAlias("инфа")
			.SetDescription("Измеряет запрошенную инфу")
			.SetUsage("<command> какая-то инфа")
			.AddTag("fun");


			CommandsController.AddCommand("popcat", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;


				var bitmap = Bronuh.Properties.Resources.Popcat;
				MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
				memoryStream.Position = 0;

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithFile("Popcat.gif", memoryStream));
			})
			.AddAlias("попкот")
			.SetDescription("Покажет тебе поп-кета")
			.SetUsage("<command>")
			.AddTag("fun");
		}
	}
}
