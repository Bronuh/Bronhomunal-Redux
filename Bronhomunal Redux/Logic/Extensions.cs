using System;
using System.Collections.Generic;
using System.Text;
using Bronuh.Types;
using DSharpPlus.Entities;

namespace Bronuh
{
    public static class Extensions
    {
        public static bool IsOwner(this DiscordUser user)
        {
            return user.Id == 263705631549161472;
        }

        public static bool IsOwner(this DiscordMember member)
        {
            return member.Id == 263705631549161472;
        }

		public static DiscordMember ToDiscordMember(this DiscordUser user)
		{
			if (Bot.Ready)
			{
				foreach (DiscordMember member in Bot.DiscordMembers)
				{
					if (member.Id == user.Id)
					{
						return member;
					}
				}
			}

			return null;
		}

		public static Member ToMember(this DiscordMember user)
		{
			if (Bot.Ready)
			{
				return MembersController.FindMember(user.Id);
			}

			return null;
		}

		public static Member ToMember(this DiscordUser user)
		{
			if (Bot.Ready)
			{
				return MembersController.FindMember(user.Id);
			}

			return null;
		}
	}
}
