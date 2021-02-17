using Bronuh.Libs;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Controllers.Commands
{
	class Memes : ICommands
	{
		private int _hmmId = 1;
		private int _okayId = 1;
		public void InitializeCommands()
		{
			CommandsController.AddCommand("killfrog", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithFile("Killfrog.gif", Properties.Memes.Killfrog.ToGifStream()));
			})
			.AddAlias("киллфрог")
			.SetDescription("Постит киллфрога")
			.AddTag("fun")
			.AddTag("meme")
			.AddTag("gif");


			CommandsController.AddCommand("hmm", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();
				int maxHmm = 15 + 1;

				var prop = typeof(Properties.Memes).GetProperty("Hmm"+  _hmmId);
				var bmp = (System.Drawing.Bitmap)prop.GetValue(null);
				builder.WithFile("Hmm.png", bmp.ToStream());
				_hmmId++;
				_hmmId = _hmmId == maxHmm ? 1 : _hmmId;

				await m.RespondAsync(builder);
			})
			.AddAliases("хмм", "думает", "думоет")
			.SetDescription("Постит мыслительный процесс")
			.AddTag("fun")
			.AddTag("meme")
			.AddTag("png");

			CommandsController.AddCommand("сойдет", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();
				int maxOkay = 2 + 1;
				var prop = typeof(Properties.Memes).GetProperty("GoodEnough" +  _okayId);
				var bmp = (System.Drawing.Bitmap)prop.GetValue(null);
				builder.WithFile("ItsOkay.png", bmp.ToStream());
				_okayId++;
				_okayId = _okayId == maxOkay ? 1 : _okayId;


				await m.RespondAsync(builder);
			})
			.AddAliases("итаксойдет", "похуй", "норм")
			.SetDescription("Вам, вероятно, похуй")
			.AddTag("fun")
			.AddTag("meme")
			.AddTag("png");
		}
	}
}
