﻿using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
	public class ChatMessage
	{
		public DiscordMessage Source;
		public Member Author;
		public string Text;

		public ChatMessage(DiscordMessage source)
		{
			Source = source;
			Author = MembersController.FindMember(source.Author.Id);
			Text = source.Content;
		}

		public ChatMessage(string text)
		{
			Source = null;
			Author = new Member()
			{
				IsOP = true,
				Rank = Int32.MaxValue,
				Id = 0,
				DisplayName = "CONSOLE",
				Discriminator = "0000",
				Nickname = "CONSOLE",
				Username = "CONSOLE",
				XP = Int32.MaxValue
			};
			Text = text;
		}
	}
}