using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Bronuh.Types;
using DSharpPlus.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Bronuh
{
    public static class Extensions
    {
        public static bool IsOwner(this DiscordUser user)
        {
            return user.Id == 263705631549161472;
        }


        public static bool IsOwner(this DiscordMember member)
        {
            return member.Id == 263705631549161472;
        }


		public static DiscordMember ToDiscordMember(this DiscordUser user)
		{
			if (Bot.Ready)
			{
				foreach (DiscordMember member in Bot.DiscordMembers)
				{
					if (member.Id == user.Id)
					{
						return member;
					}
				}
			}

			return null;
		}


		public static Member ToMember(this DiscordMember user)
		{
			if (Bot.Ready)
			{
				return MembersController.FindMember(user.Id);
			}

			return null;
		}


		public static Member ToMember(this DiscordUser user)
		{
			if (Bot.Ready)
			{
				return MembersController.FindMember(user.Id);
			}

			return null;
		}

		public static string ToLine(this List<string> list)
		{
			string respond = "";
			foreach (string word in list)
				respond += word + (word == list[^1] ? "" : ", ");

			return respond;
		}

		public static T GetRandom<T>(this List<T> list)
		{
			return list[new Random().Next(0,list.Count-1)];
		}


		public static bool HasRole(this Member member, DiscordRole role)
		{
			if (member.Source != null)
			{
				foreach (DiscordRole memberRole in member.Source.Roles)
				{
					if (memberRole.Name == role.Name) return true;
				}
			}
			return false;
		}


		public static bool HasRole(this DiscordMember member, DiscordRole role)
		{
			if (member != null)
			{
				foreach (DiscordRole memberRole in member.Roles)
				{
					if (memberRole.Name == role.Name) return true;
				}
			}
			return false;
		}


		public static byte[] ToArray(this Bitmap bitmap)
		{
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
			return memoryStream.ToArray();
		}


		public static Stream ToStream(this Bitmap bitmap)
		{
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
			memoryStream.Position = 0;
			return memoryStream;
		}


		// This method can be seen as an inline implementation of an `IImageProcessor`:
		// (The combination of `IImageOperations.Apply()` + this could be replaced with an `IImageProcessor`)
		public static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext ctx, float cornerRadius)
		{
			SixLabors.ImageSharp.Size size = ctx.GetCurrentSize();
			IPathCollection corners = BuildCorners(size.Width, size.Height, cornerRadius);

			ctx.SetGraphicsOptions(new GraphicsOptions()
			{
				Antialias = true,
				AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // enforces that any part of this shape that has color is punched out of the background
			});

			// mutating in here as we already have a cloned original
			// use any color (not Transparent), so the corners will be clipped
			foreach (var c in corners)
			{
				ctx = ctx.Fill(SixLabors.ImageSharp.Color.Red, c);
			}
			return ctx;
		}

		public static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
		{
			// first create a square
			var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

			// then cut out of the square a circle so we are left with a corner
			IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

			// corner is now a corner shape positions top left
			//lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

			float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
			float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

			// move it across the width of the image - the width of the shape
			IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
			IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
			IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

			return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
		}


		public static bool HasMember(this IEnumerable<DiscordMember> list, DiscordMember find)
		{
			foreach (DiscordMember member in list)
			{
				if (member.Id==find.Id)
				{
					return true;
				}
			}
			return false;
		} 

	}
}
