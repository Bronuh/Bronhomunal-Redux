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
		public int Rank = 1;
		public int XP = 0;

		public string Username, DisplayName, Discriminator, Nickname;

		public bool IsOP = false;

		public Hero Character;
		
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


		public bool IsConsole()
		{
			return (Id==0&&IsOP&&Discriminator=="0000"&&Username=="CONSOLE");
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

				//IsOP = Source.IsOwner;
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
