using RPGCore.Gameplay.Abilities;
using System;
using System.Collections.Generic;

namespace RPGCore.Entities.Items
{
	[Serializable]
	public class Effected : Item
	{
		public List<Ability> abilities = new List<Ability>();
	}
}
