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
	class AchievementBuilder
	{
		/// <summary>
		/// Создает изображение достижения
		/// </summary>
		/// <param name="achievement"></param>
		/// <returns></returns>
		public static Stream Build(Achievement achievement)
		{
			
			MemoryStream memoryStream = new MemoryStream();

			int iconSize = 200;
			int cornerRadius = 10;
			int borderSize = 15;

			int baseHeight = 256;
			int baseWidth = 1280;

			using (var baseImage = Image.Load(Properties.Achievements.Background.ToArray()))
			{
				using (var iconImage = Image.Load(Properties.Achievements.Empty.ToArray()))
				{
					using (var iconBGImage = Image.Load(Properties.Achievements.IconBackground.ToArray()))
					{
						iconBGImage.Mutate(ctx => {
							ctx.Resize(new Size(iconSize+borderSize,iconSize+borderSize))
							.ApplyRoundedCorners(cornerRadius)
							.Hue(65)
							.Brightness(3)
							.Saturate(3)
							.Lightness(1);
						});

						iconImage.Mutate(ctx => {
							ctx.Resize(new Size(iconSize, iconSize))
							.ApplyRoundedCorners(cornerRadius);
						});

						baseImage.Mutate(ctx => {
							ctx.DrawImage(iconBGImage, 
								new Point(baseHeight/2-iconBGImage.Height/2,baseHeight/2-iconBGImage.Height/2),
								1);
							ctx.DrawImage(iconImage,
								new Point(baseHeight / 2 - iconImage.Height / 2, baseHeight / 2 - iconImage.Height / 2),
								1);
							ctx.DrawText(new TextGraphicsOptions
							{
								TextOptions = {
									HorizontalAlignment = HorizontalAlignment.Center,
								}
							},
							achievement.Name + "",
							SystemFonts.CreateFont("Arial", 60),
							new Color(new Rgba32(255, 255, 100)),
							new PointF(300, 30));
						});

						baseImage.SaveAsPng(memoryStream);

						memoryStream.Position = 0;


						return memoryStream;
					}
						
				}

			}

				

			
			
		}
	}
}
