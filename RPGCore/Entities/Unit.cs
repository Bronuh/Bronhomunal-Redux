using RPGCore.Entities.Items;
using RPGCore.Events;
using RPGCore.Gameplay;
using RPGCore.Gameplay.Effects;
using System;
using System.Collections.Generic;

namespace RPGCore.Entities
{
	public delegate void DamageHandler(object sender);

	[Serializable]
	public class Unit
	{
		public event TakenDamageEventHandler UnitTakeDamage;
		public event BeforeAttackEventHandler BeforeAttack;
		public event AftereAttackEventHandler AfterAttack;
		public event ItemTakenEventHandler ItemTaken;
		public event ItemDroppedEventHandler ItemDropped;
		public event UnitDiedEventHandler UnitDied;
		public event UnitSpawnedEventHandler UnitSpawned;
		public event UnitCastingSpellEventHandler UnitCastingSpell;
		public event UnitThinkingEventHandler UnitThinking;
		public event AddedEffectEventHandler AddedEffect;
		public event RemovedEffectEventHandler RemovedEffect;

		/// <summary>
		/// Название юнита
		/// </summary>
		public string Name = "Generic Unit";
		/// <summary>
		/// Описание юнита
		/// </summary>
		public string Description = "Generic Unit Description";

		/// <summary>
		/// Список тегов юнита
		/// </summary>
		public List<string> Tags { get; }

		/// <summary>
		/// Базовый наносимый урон
		/// </summary>
		public Damage
			BaseDamage = new Damage(10),
			TakenDamage;
		public double TakenDamageAmount;
		public double
			Health = 100,
			Energy = 100,
			Armor = 0,
			Shield = 0,
			MaxHealth, MaxEnergy, MaxShield,
			HealthRegen = 1,
			EnergyRegen = 1,
			ShieldRegen = 1;

		public int Level = 1;
		public int
			HealthRegenTicks = 1,
			ShieldRegenTicks = 1;
		public List<Effect> Effects = new List<Effect>();
		public Inventory Inventory = new Inventory() { Limited = false, Size = 10 };

		public event DamageHandler DamageTaken;

		public virtual void Think() { }

		public virtual void InflictDamage(Unit target)
		{
			Damage damage = BaseDamage.Copy();
			damage.Source = this;
			foreach (Item item in Inventory.Items)
			{
				if (item is Weapon weapon)
				{
					damage.Add(weapon.WeaponDamage);
				}
			}

			BeforeAttack?.Invoke(this, new BeforeAttackEventArgs());
			target.TakeDamage(damage);
			AfterAttack?.Invoke(this, new AftereAttackEventArgs());
		}

		public virtual void TakeDamage(double amount)
		{
			Damage damage = new Damage() { BaseDamage = amount, Source = Game.World };
			TakeDamage(damage);
		}

		public virtual void TakeDamage(Damage damage)
		{
			if (!HasTag("immortal"))
			{
				TakenDamage = damage;
				double ShieldDamage = damage.GetTag("shield").Value;
				double NormalDamage = damage.BaseDamage;
				TakenDamageAmount = 0;

				foreach (TagDamage tag in damage.Tags)
					if (HasTag(tag.Tag))
						NormalDamage += tag.Value;

				if (Shield > 0)
				{
					if (damage.HasTag("shield"))
					{
						TakenDamageAmount += Math.Min(Shield, ShieldDamage);
						Shield -= ShieldDamage;
						Shield = Math.Max(0, Shield);
					}
					if (Shield > 0)
					{
						if (Shield >= NormalDamage)
						{
							TakenDamageAmount += NormalDamage;
							Shield -= NormalDamage;
							NormalDamage = 0;
						}
						else
						{
							TakenDamageAmount += Shield;
							NormalDamage -= Shield;
							Shield = 0;
						}
					}
					TakenDamageAmount += Math.Max(NormalDamage - Armor, 0.25);
					Health -= Math.Max(NormalDamage - Armor, 0.25);
				}
			}
			UnitTakeDamage?.Invoke(this, new TakenDamageEventArgs());
		}

		public virtual void ApplyEffect(Effect effect)
		{
			Effects.Add(effect);
			effect.Apply(this);
			AddedEffect?.Invoke(this, new AddedEffectEventArgs());
		}

		public virtual void RemoveEffect(Effect effect)
		{
			effect.Remove();
			Effects.Remove(effect);
			RemovedEffect?.Invoke(this, new RemovedEffectEventArgs());
		}

		public virtual bool HasTag(string tag)
		{
			foreach (String s in Tags)
			{
				if (s.ToLower().Equals(tag.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		public virtual void RemoveTag(string tag)
		{
			foreach (String s in Tags)
			{
				if (s.ToLower().Equals(tag.ToLower()))
				{
					Tags.Remove(s);
				}
			}
		}

		public virtual void AddTag(string tag)
		{
			if (!HasTag(tag))
			{
				Tags.Add(tag);
			}
		}
	}
}
