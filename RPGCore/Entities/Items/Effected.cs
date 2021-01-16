using RPGCore.Gameplay.Abilities;
using RPGCore.Gameplay.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Entities.Items
{
    [Serializable]
    public class Effected
    {
        public List<Ability> abilities = new List<Ability>();
    }
}
