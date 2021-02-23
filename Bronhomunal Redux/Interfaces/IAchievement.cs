using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bronuh.Interfaces
{
	public interface IAchievement
	{
		public Rarity Rarity { get; set; }
		public string Name { get; set; }
		public Color BorderColor { get; set; }

		public Image GetIcon();
		public string GetDescription();

		public byte[] GetBackground();

		public Stream GetImage();
	}
}
