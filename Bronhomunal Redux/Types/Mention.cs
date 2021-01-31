using Bronuh.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
    public class Mention
    {
        public String Prefix, Suffix, Message;
        public int Rank;
        public Action<Member, Member> CustomAction = (s,t)=> { };

        public Mention()
        {
            MentionsController.Mentions.Add(this);
        }

        public bool Match(string parse)
        {
            Logger.Debug(parse + " startsWith " + Prefix + " == " + parse.StartsWith(Prefix));
            Logger.Debug(parse + " endsWith " + Suffix + " == " + parse.EndsWith(Suffix));
            if (parse.StartsWith(Prefix) && parse.EndsWith(Suffix))
            {
                return true;
            }

            return false;
        }

        public string GetClearText(string parse)
        {
            string _return = parse;
            if (Prefix != "")
            {
                _return = _return.Replace(Prefix, "");
            }

            if (Suffix != "")
            {
                _return = _return.Replace(Suffix, "");
            }

            return _return;
        }
    }
}
