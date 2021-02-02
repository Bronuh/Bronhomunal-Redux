using System;

namespace RPGCore.Entities
{
	[Serializable]
	public class Item
	{
		public enum ItemType
		{
			ARMOR,
			CONSUMABLE,
			EFFECTED,
			USEABLE,
			WEAPON
		}

		public ItemType Type;
		public String Name;
		public int Size = 1;

	}
}
