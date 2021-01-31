using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using RPGCore.Entities;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using Bronuh.Controllers;

namespace Bronuh.Types
{
	[Serializable]
	public class Member
	{
		public string Username, DisplayName, Discriminator, Nickname, About;

		public ulong Id;
		public int Rank = 1;
		public int XP = 0;
		public static readonly int XpPerRank = 100;

		public bool IsOP = false;

		public int CharacterId = 0;
		
		[System.Xml.Serialization.XmlIgnore]
		public DiscordMember Source;

		[System.Xml.Serialization.XmlIgnore]
		public ChatMessage LastMessage = null;

		public List<string> Achievements = new List<string>();

		public MemberStatistics Statistics = new MemberStatistics();

		public Member() { }

		public Member(DiscordUser user) {
			Id = user.Id;
			Username = user.Username;
			Discriminator = user.Discriminator;
			About = "";
		}


		public Member(DiscordMember member) 
		{
			Source = member;
			About = "";

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
				for (int i = 1; i <= levels;i++)
				{
					await RankUpAsync();
				}
			}
		}


		public bool IsOp()
		{
			return IsOP || IsOwner() || IsBronomunal() || IsConsole();
		}


		public static int RankForXp(int xp)
		{
			return (int)Math.Floor((double)xp / XpPerRank) + 1;
		}


		public static int XpForRank(int rank)
		{
			return (rank-1) * XpPerRank;
		}


		private async Task RankUpAsync()
		{
			Rank++;
			await LastMessage?.RespondAsync($">>> :up: {DisplayName} получил ранг {Rank}!11!!");
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

				if (About == null)
				{
					About = "Пользователь ничего не написал о себе";
				}
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
				return Rank >= mention.Rank;
			}
		}


		public Bitmap GetAvatar()
		{
			WebClient client = new WebClient();
			Stream stream = client.OpenRead(Source.AvatarUrl);
			Bitmap bitmap;
			bitmap = new Bitmap(stream);

			stream.Flush();
			stream.Close();
			client.Dispose();

			return bitmap;

		}


		public Stream GetBasicProfileImageStream()
		{
			return Graphics.SmallProfileBuilder.Build(this);
		}

		public bool HasAchievement(string id)
		{
			return Achievements.Contains(id.ToLower());
		}

		public bool HasAchievement(Achievement achievement)
		{
			return HasAchievement(achievement.Id);
		}

		public async Task GetAchievement(string id)
		{
			

			Achievement achievement = AchievementsController.Find(id);

			if (achievement!=null)
			{
				if (!HasAchievement(achievement)) {
					Achievements.Add(id.ToLower());
					await LastMessage?.RespondAsync(new DiscordMessageBuilder()
						.WithContent(":pencil: " + Source.Mention + " получил достижение!")
						.WithFile(achievement.Name + ".png", achievement.GetImage()));

					if (HasAchievement("stickpoke10times")
						&& HasAchievement("stickhit20times")
						&& HasAchievement("loghit20times")
						&& HasAchievement("treehit30times"))
					{
						await GetAchievement("woodenwarrior");
					}
				}
			}
		}
	}
}
