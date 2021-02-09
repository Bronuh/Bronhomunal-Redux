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
		}
	}
}
