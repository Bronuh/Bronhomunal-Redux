using System;

namespace RPGCore.Events
{
	public delegate void EventHandler(object sender, EventArgs eventArgs);

	public delegate void TakenDamageEventHandler(object sender, TakenDamageEventArgs eventArgs);
	public class TakenDamageEventArgs : EventArgs
	{

	}

	public delegate void BeforeAttackEventHandler(object sender, BeforeAttackEventArgs eventArgs);
	public class BeforeAttackEventArgs : EventArgs
	{

	}

	public delegate void AftereAttackEventHandler(object sender, AftereAttackEventArgs eventArgs);
	public class AftereAttackEventArgs : EventArgs
	{

	}

	public delegate void ItemTakenEventHandler(object sender, ItemTakenEventArgs eventArgs);
	public class ItemTakenEventArgs : EventArgs
	{

	}

	public delegate void ItemDroppedEventHandler(object sender, ItemDroppedEventArgs eventArgs);
	public class ItemDroppedEventArgs : EventArgs
	{

	}

	public delegate void UnitDiedEventHandler(object sender, UnitDiedEventArgs eventArgs);
	public class UnitDiedEventArgs : EventArgs
	{

	}

	public delegate void UnitSpawnedEventHandler(object sender, UnitSpawnedEventArgs eventArgs);
	public class UnitSpawnedEventArgs : EventArgs
	{

	}


	public delegate void HeroLevelUpEventHandler(object sender, HeroLevelUpEventArgs eventArgs);
	public class HeroLevelUpEventArgs : EventArgs
	{

	}

	public delegate void HeroRessurectedEventHandler(object sender, HeroRessurectedEventArgs eventArgs);
	public class HeroRessurectedEventArgs : EventArgs
	{

	}

	public delegate void UnitCastingSpellEventHandler(object sender, UnitCastingSpellEventArgs eventArgs);
	public class UnitCastingSpellEventArgs : EventArgs
	{

	}

	public delegate void HeroSoldEventHandler(object sender, HeroSoldItemEventArgs eventArgs);
	public class HeroSoldItemEventArgs : EventArgs
	{

	}

	public delegate void HeroBoughtItemHandler(object sender, HeroBoughtItemEventArgs eventArgs);
	public class HeroBoughtItemEventArgs : EventArgs
	{

	}

	public delegate void UnitThinkingEventHandler(object sender, UnitThinkingEventArgs eventArgs);
	public class UnitThinkingEventArgs : EventArgs
	{

	}

	public delegate void AddedEffectEventHandler(object sender, AddedEffectEventArgs eventArgs);
	public class AddedEffectEventArgs : EventArgs
	{

	}

	public delegate void RemovedEffectEventHandler(object sender, RemovedEffectEventArgs eventArgs);
	public class RemovedEffectEventArgs : EventArgs
	{

	}
}
