﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Entities.Items
{
    [Serializable]
    public class Weapon : Item
    {
        public Damage WeaponDamage;
    }
}
