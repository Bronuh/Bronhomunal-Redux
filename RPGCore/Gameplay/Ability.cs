using System;

namespace RPGCore.Gameplay.Abilities
{
	[Serializable]
	public class Ability
	{
		public enum AbilityType
		{
			ACTIVE,
			PASSIVE
		}
		public enum AbilityTargetType
		{
			SELF,
			ALLY,
			ENEMY,
			OTHER
		}
		public enum AbilityEventType
		{
			OnAttack,
			OnDamage,
			OnCast,
			OnMove,
			Always
		}
		public string Name;
		public string Description;

		public AbilityType Type;
		public AbilityTargetType TargetType;
		public AbilityEventType EventType;

	}
}
