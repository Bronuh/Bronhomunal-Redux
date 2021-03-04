using Bronuh;
using Bronuh.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bronuh.Types
{
	[Serializable]
	public class CustomAchievement : IAchievement
	{
		private Stream _cache;
		private bool _cached = false;
		public Rarity Rarity { get; set; }
		public string Name { get; set; }
		[XmlIgnore]
		public System.Drawing.Image Icon { get; set; }
		public Color BorderColor { get; set; }

		public string ImageLink; 
		public string Description = "Эксклюзивное админское дерьмо!";

		public CustomAchievement() { }

		public CustomAchievement SetName(string name)
		{
			Name = name;
			return this;
		}

		public Color GetColor()
		{
			switch (Rarity)
			{
				case Rarity.COMMON:
					return BorderColor = Color.FromArgb(150, 150, 150);
				case Rarity.UNCOMMON:
					return BorderColor = Color.LimeGreen;
				case Rarity.RARE:
					return BorderColor = Color.DodgerBlue;
				case Rarity.LEGENDARY:
					return BorderColor = Color.DarkViolet;
				case Rarity.EXOTIC:
					return BorderColor = Color.Gold;
				case Rarity.UNREAL:
					return BorderColor = Color.Red;
				default:
					return BorderColor = Color.FromArgb(150, 150, 150);
			}
		}
		public CustomAchievement SetRarity(Rarity rarity)
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

		public CustomAchievement SetDescription(string description)
		{
			Description = description;
			return this;
		}

		public CustomAchievement SetImage(string link)
		{
			ImageLink = link;
			GetIcon();
			return this;
		}

		public Image GetIcon()
		{
			if (Icon == null)
			{
				Icon = Image.FromStream(new MemoryStream(new WebClient().DownloadData(ImageLink)));
			}
			return Icon;
		}

		public string GetDescription()
		{
			return Description;
		}


		public byte[] GetBackground()
		{
			return Properties.Achievements.CustomBackground.ToArray();
		}


		public Stream GetImage()
		{
			if (!_cached)
			{
				_cache = Bronuh.Graphics.AchievementBuilderTest.Build(this);
				_cached = true;
			}

			_cache.Position = 0;
			return _cache;
		}
	}
}
