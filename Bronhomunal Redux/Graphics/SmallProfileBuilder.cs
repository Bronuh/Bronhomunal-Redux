using Bronuh.Types;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace Bronuh.Graphics
{
	public static class SmallProfileBuilder
	{

		public static Stream Build(Member member)
		{

			var avatarImage = Image.Load(member.GetAvatar().ToArray());
			var baseImage = Image.Load(Bronuh.Properties.Resources.Level.ToArray());
			var crownImage = Image.Load(Bronuh.Properties.Resources.Crown.ToArray());



			/// Prepare avatar
			avatarImage.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new Size(110, 110),
				Mode = ResizeMode.Crop
			}).ApplyRoundedCorners(10));



			///Base info
			baseImage.Mutate(ctx =>
			{
				int step = (128 - 110) / 2;
				ctx.DrawImage(avatarImage, new Point(720 - 110 - step, step), 1);
				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Center,
						}
				},
				member.Rank + "",
				SystemFonts.CreateFont("Arial", 60),
				new Color(new Rgba32(255, 255, 255)),
				new PointF(69, 30));

				var col = member.Source.Color;


				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
						}
				},
				member.Username,
				SystemFonts.CreateFont("Arial", 50),
				new Color(new Rgba32(col.R, col.G, col.B)),
				new PointF(150, 5));



				ctx.DrawLines(new Pen(
					new Color(
						new Rgba32(100, 100, 100)), 3),
					new PointF[] {
						new PointF(155,64),
						new PointF(590,64)
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
				SystemFonts.CreateFont("Arial", 14),
				new Color(new Rgba32(255, 255, 255)),
				new PointF(160, 68));
				//new SixLabors.ImageSharp.PointF(150, 33));
				/// XP Bar

				int curXp = member.XP - Member.XpForRank(member.Rank);
				int xPos = 250;
				int length = 340;
				int yPos = 120;

				ctx.DrawLines(new Pen(
				new Color(
					new Rgba32(100, 100, 100)), 7),
				new PointF[] {
						new PointF(xPos, yPos),
						new PointF(xPos+length, yPos)
				});

				ctx.DrawLines(new Pen(
				new Color(
					new Rgba32(255, 255, 255)), 3f),
				new PointF[] {
						new PointF(xPos, yPos),
						new PointF(xPos+length*((float)curXp/Member.XpPerRank), yPos)
				});

				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
							WrapTextWidth = 430
						}
				},
				"XP: " + member.XP + " / " + Member.XpForRank(member.Rank + 1),
				SystemFonts.CreateFont("Arial", 14),
				new Color(new Rgba32(255, 255, 255)),
				new PointF(xPos - 100, yPos - 8));

			});



			if (member.IsOp())
			{


				crownImage.Mutate(ctx =>
				{
					ctx.Resize(new Size(65, 65));
				});

				baseImage.Mutate(ctx =>
				{
					int step = (128 - 110) / 2;
					ctx.DrawImage(crownImage, new Point(720 - 150, -10), 1);

				});
			}





			baseImage.Mutate(x => x.ApplyRoundedCorners(10));

			MemoryStream memoryStream = new MemoryStream();
			baseImage.SaveAsPng(memoryStream);
			memoryStream.Position = 0;

			baseImage.Dispose();
			avatarImage.Dispose();
			crownImage.Dispose();

			return memoryStream;
		}
	}
}
