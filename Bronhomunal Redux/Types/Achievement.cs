using System.Drawing;
using System.IO;

public enum Rarity
{
	COMMON = 1,
	UNCOMMON,
	RARE,
	LEGENDARY,
	EXOTIC
}
namespace Bronuh.Types
{
	public class Achievement
	{
		public string Id;
		public string Name;
		public string Description;
		public Image Icon;
		public int ColorShift = 0;
		public int Lightness = 0;
		public Color BorderColor = Color.FromArgb(150, 150, 150);
		public static int BaseXP = 5;
		public Rarity Rarity = Rarity.COMMON;

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
	}
}
