using RPGCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Gameplay.Effects
{
    [Serializable]
    public class Effect
    {
        public enum Type
        {
            ITEM,
            ABILITY,
            OTHER
        }

        public string Name, Description;
        public Type SourceType;
        public int TicksLeft;
        public Item Source;
        public Unit Target;

        public virtual void Apply(Unit target)
        {
            Target = target;
        }

        public virtual void Remove()
        {
            // WHat to change in stats
        }
    }
}
