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

	class RankUpBuilder
	{
		/// <summary>
		/// Возвращает картинку-уведомление о повышении ранга
		/// </summary>
		/// <param name="member">Участник, чей ранг повышается</param>
		/// <param name="rank">Какую циферку надо написать. Если 0, то напишет реальный ранг пользователя</param>
		/// <returns></returns>
		public static Stream Build(Member member, int rank = 0)
		{
			var avatarImage = Image.Load(member.GetAvatar().ToArray());
			var baseImage = Image.Load(Bronuh.Properties.Resources.Level.ToArray());
			var arrowImage = Image.Load(Bronuh.Properties.Resources.ArrowGlowing.ToArray());
			
			
			if (rank == 0)
			{
				rank = member.Rank;
			}

			/// Подготовка аватарки
			avatarImage.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new Size(110, 110),
				Mode = ResizeMode.Stretch
			}).ApplyRoundedCorners(10));


			/// Подготовка изображения стрелочки
			arrowImage.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new Size(48, 64),
				Mode = ResizeMode.Stretch
			}));


			///Основное изображение
			baseImage.Mutate(ctx => {
				int step = (128 - 110) / 2;
				ctx.DrawImage(avatarImage, new Point(720 - 110 - step, step), 1);
				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Center,
						}
				},
				rank + "",
				SystemFonts.CreateFont("Arial", 60),
				new Color(new Rgba32(255, 255, 255)),
				new PointF(70, 30));

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

				ctx.DrawText(new TextGraphicsOptions
				{
					TextOptions = {
							HorizontalAlignment = HorizontalAlignment.Left,
						}
				},
				"Повысил ранг до "+rank,
				SystemFonts.CreateFont("Arial", 30),
				new Color(new Rgba32(255,255,255)),
				new PointF(150, 75));


				/// Черта под ником
				ctx.DrawLines(new Pen(
					new Color(new Rgba32(100, 100, 100)), 3),
					new PointF[] {
						new PointF(155,64),
						new PointF(590,64)
					});

			});

			/// Отрисовка стрелочки у ника
			baseImage.Mutate(ctx =>
			{
				ctx.DrawImage(arrowImage, new Point(110, 5), 1);
			});


			/// Добавление короны к аватарке для админов
			if (member.IsOp())
			{
				var crownImage = Image.Load(Bronuh.Properties.Resources.Crown.ToArray());

				crownImage.Mutate(ctx =>
				{
					ctx.Resize(new Size(65, 65));
				});

				baseImage.Mutate(ctx =>
				{
					int step = (128 - 110) / 2;
					ctx.DrawImage(crownImage, new Point(720 - 150, -10), 1);
				});

				crownImage.Dispose();
			}


			/// Завершение отрисовки и возврат изображения

			baseImage.Mutate(x => x.ApplyRoundedCorners(10));

			MemoryStream memoryStream = new MemoryStream();
			baseImage.SaveAsPng(memoryStream);
			memoryStream.Position = 0;

			baseImage.Dispose();
			avatarImage.Dispose();
			arrowImage.Dispose();


			return memoryStream;
		}
	}
}
