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
using Bronuh.Graphics;

namespace Bronuh.Types
{
	[Serializable]
	public class Member
	{
		public string Username, DisplayName, Discriminator, Nickname, About;

		public ulong Id;
		public int Rank = 1;
		public int XP = 0;
		public static readonly int XpPerRank = 50;

		public bool IsOP = false;

		public int CharacterId = 0;
		
		[System.Xml.Serialization.XmlIgnore]
		public DiscordMember Source;

		[System.Xml.Serialization.XmlIgnore]
		public ChatMessage LastMessage = null;

		[System.Xml.Serialization.XmlIgnore]
		public bool IsInVoice = false;

		[System.Xml.Serialization.XmlIgnore]
		public DateTime LastVoiceIn;

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


		/// <summary>
		/// Собирает основную инфу в одну строку
		/// </summary>
		/// <returns></returns>
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


		/// <summary>
		/// Выдает опыт и повышает ранг при необходимости
		/// </summary>
		/// <param name="xp"></param>
		/// <returns></returns>
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

		/// <summary>
		/// метод проверки на админовость
		/// </summary>
		/// <returns></returns>
		public bool IsOp()
		{
			return IsOP || IsOwner() || IsBronomunal() || IsConsole();
		}


		/// <summary>
		/// Определяет сколько какому рангу будет соответствовать указанно количество опыта
		/// </summary>
		/// <param name="xp"></param>
		/// <returns>Доступный ранг</returns>
		public static int RankForXp(int xp)
		{
			return (int)Math.Floor((double)xp / XpPerRank) + 1;
		}


		/// <summary>
		/// Возвращает опыт, необходимый для получения указанного ранга
		/// </summary>
		/// <param name="rank"></param>
		/// <returns>опыт</returns>
		public static int XpForRank(int rank)
		{
			return (rank-1) * XpPerRank;
		}


		/// <summary>
		/// Повышает ранг на 1, с уведомлением в чат
		/// </summary>
		/// <returns></returns>
		private async Task RankUpAsync()
		{
			Rank++;
			var msgBuilder = new DiscordMessageBuilder()
						.WithContent(":up: " + DisplayName + " повысил свой ранг!" + Program.Suffix)
						.WithFile("RankUp.png", RankUpBuilder.Build(this));
			await Bot.SendMessageAsync(msgBuilder);
		}


		/// <summary>
		/// Проверяет является ли пользователь бронухом
		/// </summary>
		/// <returns></returns>
		public bool IsOwner()
		{
			return Id == 263705631549161472;
		}

		/// <summary>
		/// Обновляет информацию о пользователе
		/// </summary>
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


		/// <summary>
		/// Проверяет может ли указанный пользователь использовать это упоминание
		/// </summary>
		/// <param name="mention"></param>
		/// <returns></returns>
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


		/// <summary>
		/// скачивает аватарку пользователя из сети
		/// </summary>
		/// <returns>Аватарка</returns>
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

		/// <summary>
		/// Возвращает шапку профиля
		/// </summary>
		/// <returns></returns>
		public Stream GetBasicProfileImageStream()
		{
			return Graphics.SmallProfileBuilder.Build(this);
		}

		/// <summary>
		/// Проверяет имеет ли пользователь достижение с указанным ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool HasAchievement(string id)
		{
			return Achievements.Contains(id.ToLower());
		}


		public void JoinedVoice()
		{
			Logger.Log(DisplayName+" подключен к войсу");
			LeavedVoice();
			IsInVoice = true;
			LastVoiceIn = DateTime.Now;
		}

		public void LeavedVoice()
		{
			if (IsInVoice)
			{
				int delay = (int)(DateTime.Now - LastVoiceIn).TotalMilliseconds;
				Logger.Log(DisplayName + " отключен от войса "+delay);
				IsInVoice = false;
				Statistics.VoiceTime += delay;
			}
		}

		public long GetVoiceTime()
		{
			int current = 0;
			if (IsInVoice)
			{
				current = (int)(LastVoiceIn - DateTime.Now).TotalMilliseconds;
			}
			return Statistics.VoiceTime+current;
		}

		/// <summary>
		/// Проверяет имеет ли пользователдь данное достижение
		/// </summary>
		/// <param name="achievement"></param>
		/// <returns></returns>
		public bool HasAchievement(Achievement achievement)
		{
			return HasAchievement(achievement.Id);
		}

		/// <summary>
		/// Выдает пользователю достижение, если он его еще не имеет. Также может выдать дополнительные достижения самостоятельно.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task GiveAchievement(string id)
		{
			Achievement achievement = AchievementsController.Find(id);

			if (achievement!=null)
			{
				if (!HasAchievement(achievement)) {
					Achievements.Add(id.ToLower());
					await AddXPAsync((int)achievement.Rarity * Achievement.BaseXP);

					var msgBuilder = new DiscordMessageBuilder()
						.WithContent(":trophy: " + Source.Mention + " получил достижение!" + Program.Suffix)
						.WithFile(achievement.Name + ".png", achievement.GetImage());

					await Bot.BotChannel.SendMessageAsync(msgBuilder);
					

					if (HasAchievement("stickpoke")
						&& HasAchievement("stickhit")
						&& HasAchievement("loghit")
						&& HasAchievement("treehit"))
					{
						await GiveAchievement("woodenwarrior");
					}
				}
			}
		}
	}
}
