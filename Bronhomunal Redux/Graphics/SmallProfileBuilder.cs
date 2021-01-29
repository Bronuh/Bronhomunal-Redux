using Bronuh.Types;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bronuh.Graphics
{
	public static class SmallProfileBuilder
	{
		public static Stream Build(Member member)
		{

			var avatarImage = Image.Load(member.GetAvatar().ToArray());
			var baseImage = Image.Load(Bronuh.Properties.Resources.Level.ToArray());



			/// Prepare avatar
			avatarImage.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new SixLabors.ImageSharp.Size(110, 110),
				Mode = ResizeMode.Crop
			}).ApplyRoundedCorners(10));



			///Base info
			baseImage.Mutate(ctx => {
				int step = (128 - 110) / 2;
				ctx.DrawImage(avatarImage, new SixLabors.ImageSharp.Point(720 - 110 - step, step), 1);
				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Center,
						}
				},
				member.Rank + "",
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 60),
				new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
				new SixLabors.ImageSharp.PointF(68, 30));

				var col = member.Source.Color;


				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
						}
				},
				member.Username,
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 50),
				new SixLabors.ImageSharp.Color(new Rgba32(col.R, col.G, col.B)),
				new SixLabors.ImageSharp.PointF(150, 5));

				

				ctx.DrawLines(new SixLabors.ImageSharp.Drawing.Processing.Pen(
					new SixLabors.ImageSharp.Color(
						new Rgba32(100, 100, 100)), 3),
					new SixLabors.ImageSharp.PointF[] {
						new SixLabors.ImageSharp.PointF(155,64),
						new SixLabors.ImageSharp.PointF(590,64)
					});

				int maxLength = 100;
				string about = member.About;
				about = about.Length > maxLength ? about.Substring(0, maxLength - 1) + "..." : about;

				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
							WrapTextWidth = 430
						}
				},
				about,
				SixLabors.Fonts.SystemFonts.CreateFont("Arial", 14),
				new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
				new SixLabors.ImageSharp.PointF(160, 68));
				//new SixLabors.ImageSharp.PointF(150, 33));


			});
			if (member.IsOp())
			{
				var crownImage = Image.Load(Bronuh.Properties.Resources.Crown.ToArray());

				crownImage.Mutate(ctx =>
				{
					ctx.Resize(new SixLabors.ImageSharp.Size(65, 65));
				});

				baseImage.Mutate(ctx =>
				{
					int step = (128 - 110) / 2;
					ctx.DrawImage(crownImage, new SixLabors.ImageSharp.Point(720 - 150, -10), 1);

					/// XP Bar

					int curXp = member.XP - Member.XpForRank(member.Rank);
					int xPos = 250;
					int length = 340;
					int yPos = 120;

					ctx.DrawLines(new SixLabors.ImageSharp.Drawing.Processing.Pen(
					new SixLabors.ImageSharp.Color(
						new Rgba32(100, 100, 100)), 7),
					new SixLabors.ImageSharp.PointF[] {
						new SixLabors.ImageSharp.PointF(xPos, yPos),
						new SixLabors.ImageSharp.PointF(xPos+length, yPos)
					});

					ctx.DrawLines(new SixLabors.ImageSharp.Drawing.Processing.Pen(
					new SixLabors.ImageSharp.Color(
						new Rgba32(255, 255, 255)), 3f),
					new SixLabors.ImageSharp.PointF[] {
						new SixLabors.ImageSharp.PointF(xPos, yPos),
						new SixLabors.ImageSharp.PointF(xPos+length*((float)curXp/Member.XpPerRank), yPos)
					});

					ctx.DrawText(new TextGraphicsOptions
					{
						TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
							WrapTextWidth = 430
						}
					},
					"XP: " + member.XP + " / " + Member.XpForRank(member.Rank + 1),
					SixLabors.Fonts.SystemFonts.CreateFont("Arial", 14),
					new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
					new SixLabors.ImageSharp.PointF(xPos - 100, yPos - 8));

				});
			}

			baseImage.Mutate(x => x.ApplyRoundedCorners(10));

			MemoryStream memoryStream = new MemoryStream();
			baseImage.SaveAsPng(memoryStream);
			memoryStream.Position = 0;

			return memoryStream;
		}
	}
}
