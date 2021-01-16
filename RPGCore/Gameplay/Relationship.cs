using RPGCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Gameplay
{
    [Serializable]
    public class Relationship
    {
        public Hero Target;
        public double Value;
    }
}
