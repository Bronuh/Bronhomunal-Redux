using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using RPGCore.Entities;

namespace Bronuh.Types
{
	[Serializable]
	public class Member
	{
		public ulong Id;
		public string Username, DisplayName, Discriminator, Nickname;
		public int Rank = 1;
		public Hero Character;
		public bool IsOP = false;
		public int XP = 0;

		[System.Xml.Serialization.XmlIgnore]
		public DiscordMember Source;





		public Member() { }
		public Member(DiscordUser user) {
			Id = user.Id;
			Username = user.Username;
			Discriminator = user.Discriminator;
		}
		public Member(DiscordMember member) 
		{
			Source = member;
			Character = new Hero();
			Character.Name = "Гирой";

			Update();
		}

		public void Update()
		{
			if (Source!=null)
			{
				Id = Source.Id;
				Discriminator = Source.Discriminator;
				
				Username = Source.Username;
				DisplayName = Source.DisplayName ?? Username;
				Nickname = Source.Nickname ?? Username;


				Character.CharacterName = Username;
			}
		}

		public bool CanUse(Mention mention)
		{
			if (IsOP)
			{
				return true;
			}
			else
			{
				return XP >= mention.XP;
			}
		}
	}
}
