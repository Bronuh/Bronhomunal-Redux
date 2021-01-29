using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
		private static readonly int XpPerRank = 100;

		public string Username, DisplayName, Discriminator, Nickname;

		public bool IsOP = false;

		public int Character = 0;
		
		[System.Xml.Serialization.XmlIgnore]
		public DiscordMember Source;

		[System.Xml.Serialization.XmlIgnore]
		public ChatMessage LastMessage = null;



		public Member() { }

		public Member(DiscordUser user) {
			Id = user.Id;
			Username = user.Username;
			Discriminator = user.Discriminator;
		}


		public Member(DiscordMember member) 
		{
			Source = member;

			Update();
		}


		public bool IsConsole()
		{
			return (Id==0&& IsOp()&& Discriminator=="0000"&&Username=="CONSOLE");
		}


		public bool IsBronomunal()
		{
			return Id == 696952183572267028;
		}


		public string GetInfo()
		{

			string aliases = "";
			var aliasList = AliasesController.FindAliases(Id);
			foreach (Alias alias in aliasList)
			{
				aliases += alias.Name + (alias==aliasList[^1] ? "" : ", ");
			}


			string info = $"Информация о пользователе {DisplayName} ({Username}, {Nickname}):\n" +
				$"Также известен как: {aliases} \n" +
				$"Ранг: {Rank}\n" +
				$"Опыт: {XP}\n" +
				$"Админ: {IsOp()}\n" +
				$"Консоль: {IsConsole()}\n";


			return info;
		}


		public async Task AddXPAsync(int xp)
		{
			XP += xp;
			if (RankForXp(XP) > Rank)
			{
				int levels = RankForXp(XP) - Rank;
				for (int i = 1; i <= levels;)
				{
					await RankUpAsync();
				}
			}
		}


		public bool IsOp()
		{
			return IsOP || IsOwner() || IsBronomunal() || IsConsole();
		}


		private int RankForXp(int xp)
		{
			return (int)Math.Floor((double)xp / XpPerRank) + 1;
		}


		private async Task RankUpAsync()
		{
			Rank++;
			await LastMessage?.RespondAsync($"{DisplayName} получил ранг {Rank}!11!!");
		}

		public bool IsOwner()
		{
			return Id == 263705631549161472;
		}

		public void Update()
		{
			if (Source!=null)
			{
				Id = Source.Id;
				Discriminator = Source.Discriminator;

				Username = Source.Username;
				Nickname = Source.Nickname ?? Username;
				DisplayName = Source.DisplayName ?? Nickname;
				
			}
		}



		public bool CanUse(Mention mention)
		{
			if (IsOp())
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
