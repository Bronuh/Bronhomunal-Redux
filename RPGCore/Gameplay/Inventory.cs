using RPGCore.Entities;
using System;
using System.Collections.Generic;

namespace RPGCore.Gameplay
{
	[Serializable]
	public class Inventory
	{
		public List<Item> Items = new List<Item>();
		public bool Limited = false;
		public bool Droppable = false;
		public int Size = 1;

		public bool PutItem(Item item)
		{
			if (item.Size <= GetFreeSpace())
			{
				Items.Add(item);
			}
			return false;
		}

		public void Remove(Item item)
		{
			Items.Remove(item);
		}

		public int GetFreeSpace()
		{
			return Limited ? Size - GetFilledSpace() : 100000; // Наверное, предметов, размером больше 100000 не будет. Я надеюсь...
		}

		public int GetFilledSpace()
		{
			int slots = 0;

			if (Limited)
			{
				foreach (Item item in Items)
				{
					slots += item.Size;
				}
			}

			return slots;
		}
	}
}
