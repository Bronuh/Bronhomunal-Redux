using Bronuh.Interfaces;
using System.Drawing;
using System.IO;

public enum Rarity
{
	COMMON = 1,
	UNCOMMON,
	RARE,
	LEGENDARY,
	EXOTIC,
	UNREAL
}
namespace Bronuh.Types
{
	public class Achievement : IAchievement
	{
		public string Id;
		public string Name { get; set; }
		public string Description;
		public Image Icon { get; set; }
		public int ColorShift = 0;
		public int Lightness = 0;
		public Color BorderColor { get; set; } = Color.FromArgb(150, 150, 150);
		public static int BaseXP = 10;
		public long CustomValue;
		public Rarity Rarity { get; set; } = Rarity.COMMON;

		private bool _cached = false;
		private Stream _cache;

		public Achievement() { }
		public Achievement(string id)
		{
			Id = id.ToLower();
		}

		public Achievement SetRarity(Rarity rarity)
		{
			Rarity = rarity;

			switch (rarity)
			{
				case Rarity.COMMON:
					BorderColor = Color.FromArgb(150, 150, 150);
					break;
				case Rarity.UNCOMMON:
					BorderColor = Color.LimeGreen;
					break;
				case Rarity.RARE:
					BorderColor = Color.DodgerBlue;
					break;
				case Rarity.LEGENDARY:
					BorderColor = Color.DarkViolet;
					break;
				case Rarity.EXOTIC:
					BorderColor = Color.Gold;
					break;
				case Rarity.UNREAL:
					BorderColor = Color.Red;
					break;
				default:
					BorderColor = Color.FromArgb(150, 150, 150);
					break;
			}

			return this;
		}

		public Achievement SetIcon(byte[] bytes)
		{
			Icon = Image.FromStream(new MemoryStream(bytes));
			return this;
		}

		public Achievement SetIcon(System.Drawing.Bitmap bitmap)
		{
			Icon = bitmap;
			return this;
		}

		public Achievement SetName(string name)
		{
			Name = name;
			return this;
		}

		public Achievement SetDescription(string description)
		{
			Description = description;
			return this;
		}

		public Achievement SetColorShift(int colorShift)
		{
			ColorShift = colorShift;
			return this;
		}

		public Achievement SetBrightness(int lightness)
		{
			Lightness = lightness;
			return this;
		}

		public Achievement SetValue(long value)
		{
			CustomValue = value;
			return this;
		}

		public string GetDescription()
		{
			return Description.Replace("<value>", CustomValue.ToString());
		}

		public Stream GetImage()
		{
			if (!_cached)
			{
				_cache = Graphics.AchievementBuilderTest.Build(this);
				_cached = true;
			}

			_cache.Position = 0;
			return _cache;
		}

		public Image GetIcon()
		{
			return Icon;
		}

		public byte[] GetBackground()
		{
			return Properties.Achievements.Background.ToArray();
		}
	}
}
