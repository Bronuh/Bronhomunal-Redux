using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AboutAttribute : System.Attribute
    {
        public string About { get; set; }

        public AboutAttribute()
        { }

        public AboutAttribute(string about)
        {
            About = about;
        }
    }
}
