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


namespace Bronuh.Types
{
	[Serializable]
	public class Member
	{
		public ulong Id;
		public int Rank = 1;
		public int XP = 0;
		private static readonly int XpPerRank = 100;

		public string Username, DisplayName, Discriminator, Nickname, About;

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


		private int RankForXp(int xp)
		{
			return (int)Math.Floor((double)xp / XpPerRank) + 1;
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
				return XP >= mention.XP;
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
			Bitmap baseBitmap = Bronuh.Properties.Resources.Level;
			Bitmap avatarBitmap = GetAvatar();

			
			var avatarImage = Image.Load(avatarBitmap.ToArray());
			var baseImage = Image.Load(baseBitmap.ToArray());
			


			avatarImage.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new SixLabors.ImageSharp.Size(110, 110),
				Mode = ResizeMode.Crop
			}).ApplyRoundedCorners(10));



			baseImage.Mutate(ctx => {
				int step = (128 - 110) / 2;
				ctx.DrawImage(avatarImage, new SixLabors.ImageSharp.Point(720 - 110 - step, step), 1);
				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Center,
						}
				},
				Rank + "",
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 60),
				new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
				new SixLabors.ImageSharp.PointF(68, 30));

				var col = Source.Color;

				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
						}
				},
				Username,
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 50),
				new SixLabors.ImageSharp.Color(new Rgba32(col.R, col.G, col.B)),
				new SixLabors.ImageSharp.PointF(150, 5));

				//ctx.DrawLines(new SixLabors.ImageSharp.Drawing.Processing.Pen(
				//	new SixLabors.ImageSharp.Color(
				//		new Rgba32(100, 100, 100)),3),
				//	new SixLabors.ImageSharp.PointF[] {
				//		new SixLabors.ImageSharp.PointF(155,64),
				//		new SixLabors.ImageSharp.PointF(155,120)
				//	});

				ctx.DrawLines(new SixLabors.ImageSharp.Drawing.Processing.Pen(
					new SixLabors.ImageSharp.Color(
						new Rgba32(100, 100, 100)), 3),
					new SixLabors.ImageSharp.PointF[] {
						new SixLabors.ImageSharp.PointF(155,64),
						new SixLabors.ImageSharp.PointF(590,64)
					});

				int maxLength = 100;
				string about = About;
				about = about.Length > maxLength ? about.Substring(0, maxLength-1)+"..." : about;

				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
							WrapTextWidth = 430
						}
				},
				about,
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 16),
				new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
				new SixLabors.ImageSharp.PointF(160, 68));
				//new SixLabors.ImageSharp.PointF(150, 33));


			});
			if (IsOp())
			{
				Bitmap crownBitmap = Bronuh.Properties.Resources.Crown;
				var crownImage = Image.Load(crownBitmap.ToArray());

				crownImage.Mutate(ctx =>
				{
					ctx.Resize(new SixLabors.ImageSharp.Size(65, 65));
				});

				baseImage.Mutate(ctx =>
				{
					int step = (128 - 110) / 2;
					ctx.DrawImage(crownImage, new SixLabors.ImageSharp.Point(720 - 150, -10), 1);
				});
			}

			MemoryStream memoryStream = new MemoryStream();
			baseImage.SaveAsPng(memoryStream);
			//bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			memoryStream.Position = 0;

			return memoryStream;
		}
	}
}
