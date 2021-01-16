using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
    [Serializable]
    public class Infa
    {
        public string Text;
        public double Value;

        public static Infa CheckInfo(String infa)
        {
            String text = infa.ToLower().Replace("?", "").Replace("!", "").Replace(".", "").Replace("ё", "е").Trim().Replace("   ", "").Replace("  ", " ");
            return Modules.Infameter.FindInfo(infa);
        }
    }
}
