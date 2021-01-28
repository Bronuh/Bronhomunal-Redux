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

		public static string ToLine(this List<string> list)
		{
			string respond = "";
			foreach (string word in list)
				respond += word + (word == list[^1] ? "" : ", ");

			return respond;
		}

		public static T GetRandom<T>(this List<T> list)
		{
			return list[new Random().Next(0,list.Count-1)];
		}


		public static bool HasRole(this Member member, DiscordRole role)
		{
			if (member.Source != null)
			{
				foreach (DiscordRole memberRole in member.Source.Roles)
				{
					if (memberRole.Name == role.Name) return true;
				}
			}
			return false;
		}


		public static bool HasRole(this DiscordMember member, DiscordRole role)
		{
			if (member != null)
			{
				foreach (DiscordRole memberRole in member.Roles)
				{
					if (memberRole.Name == role.Name) return true;
				}
			}
			return false;
		}
	}
}
