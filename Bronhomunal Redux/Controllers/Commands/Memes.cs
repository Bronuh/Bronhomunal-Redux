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
				

				switch (new Random().Next(1, 6))
				{
					case 1:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm1.ToStream()); ;
						break;
					case 2:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm2.ToStream()); ;
						break;
					case 3:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm3.ToStream()); ;
						break;
					case 4:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm4.ToStream()); ;
						break;
					case 5:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm5.ToStream()); ;
						break;
					case 6:
						builder.WithFile("Hmm.png", Properties.Memes.Hmm6.ToStream()); ;
						break;
				}

				await m.RespondAsync(builder);
			})
			.AddAliases("хмм", "думает", "думоет")
			.SetDescription("Постит мыслительный процесс")
			.AddTag("fun")
			.AddTag("meme")
			.AddTag("png");
		}
	}
}
