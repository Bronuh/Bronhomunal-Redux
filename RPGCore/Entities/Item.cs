using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
