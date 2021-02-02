using Bronuh.Types;
using ImageProcessor;
using ImageProcessor.Imaging;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.IO;

namespace Bronuh.Graphics
{
	class AchievementBuilderTest
	{
		/// <summary>
		/// Создает изображение достижения. 
		/// TODO: Добавить генерацию черно-белых изображений.
		/// </summary>
		/// <param name="achievement">Целевое достижение</param>
		/// <returns></returns>
		public static Stream Build(Achievement achievement)
		{
			MemoryStream memoryStream = new MemoryStream();

			int iconSize = 200;
			int cornerRadius = 25;
			int borderSize = 25;

			int baseHeight = 256;
			int baseWidth = 1280;

			ImageFactory baseBuilder = new ImageFactory().Load(Properties.Achievements.Background.ToArray());
			ImageFactory imageBuilder = new ImageFactory().Load(achievement.Icon);
			ImageFactory imageBgBuilder = new ImageFactory().Load(Properties.Achievements.IconBackground.ToArray());

			imageBuilder
				.Resize(new Size(iconSize, iconSize))
				.RoundedCorners(cornerRadius);

			imageBgBuilder
				.Resize(new Size(iconSize + borderSize, iconSize + borderSize))
				.RoundedCorners((int)(cornerRadius * 1.5))
				.ReplaceColor(Color.FromArgb(255, 0, 0), achievement.BorderColor, 5);

			Image iconBgImage = imageBgBuilder.Image;
			Image iconImage = imageBuilder.Image;

			baseBuilder.Load(Properties.Achievements.Background.ToStream())
				.Overlay(new ImageLayer()
				{
					Image = iconBgImage,
					Size = new Size(iconSize + borderSize, iconSize + borderSize),
					Position = new Point(baseHeight / 2 - iconBgImage.Height / 2, baseHeight / 2 - iconBgImage.Height / 2)
				})
				.Overlay(new ImageLayer()
				{
					Image = iconImage,
					Size = new Size(iconSize, iconSize),
					Position = new Point(baseHeight / 2 - iconImage.Height / 2, baseHeight / 2 - iconImage.Height / 2)
				})
				.RoundedCorners(cornerRadius)
				.Watermark(new TextLayer()
				{
					Text = achievement.Name,
					FontFamily = new System.Drawing.FontFamily("Arial"),
					FontColor = achievement.BorderColor,
					FontSize = 60,
					Position = new Point(300, 30)
				})
				.Watermark(new TextLayer()
				{
					Text = achievement.Rarity + " (" + (Achievement.BaseXP * (int)achievement.Rarity) + " XP)",
					FontFamily = new System.Drawing.FontFamily("Arial"),
					FontColor = achievement.BorderColor,
					FontSize = 30,
					Style = System.Drawing.FontStyle.Bold,
					Position = new Point(950, 10)
				});

			baseBuilder.Save(memoryStream);

			using (var baseImage = SixLabors.ImageSharp.Image.Load(memoryStream))
			{
				baseImage.Mutate(ctx =>
				{
					ctx.DrawText(new TextGraphicsOptions
					{
						TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
							WrapTextWidth = 900
						}
					},
					achievement.Description + "",
					SixLabors.Fonts.SystemFonts.CreateFont("Arial", 37),
					new SixLabors.ImageSharp.Color(new Rgba32(255, 255, 255)),
					new SixLabors.ImageSharp.PointF(300, 125));

				});

				var outStream = new MemoryStream();
				SixLabors.ImageSharp.ImageExtensions.SaveAsPng(baseImage, outStream);
				outStream.Position = 0;

				return outStream;
			}
		}
	}
}
