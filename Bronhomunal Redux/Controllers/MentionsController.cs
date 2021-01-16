using Bronuh.Types;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bronuh.Controllers
{
	public class MentionsController
	{
        private static bool _initialized = false;
        public static List<Mention> Mentions { get; private set; } = new List<Mention>();


        public static async Task<bool> Execute(MessageCreateEventArgs e)
        {
			if (!_initialized)
			{
                InitializeMentions();
                _initialized = true;
            }

            Logger.Debug("Сообщение передано в MentionsController.Execute(s)");
            string text = e.Message.Content;
            string[] words = text.Split(' ');
            string msg = "";
            bool mentioned = true;

            foreach (string word in words)
            {
                if (mentioned)
                {
                    foreach (Mention mention in Mentions)
                    {
                        if (mention.Match(word))
                        {
                            Member author = MembersController.FindMember(e.Author.Id);
                            if (author.CanUse(mention))
                            {
                                Logger.Debug("Найдено соответствие (" + mention.GetClearText(word) + ")");
                                Member member = MembersController.FindMember(mention.GetClearText(word));
                                Logger.Debug("Найденный пользователь: " + member.Username);
                                msg += mention.Message.Replace("%MENTION%", "<@!" + member.Id + ">") + "\n";
                            }
                            else
                            {
                                msg += "Недостаточно опыта для этого действия (" + author.XP + "/" + mention.XP + ")\n";
                            }


                            mentioned = true;
                            break;
                        }
                        Logger.Debug("mentioned = false");
                        mentioned = false;
                    }
                }
                else
                {
                    break;
                }

            }

            if (msg != "")
            {
                await Bot.SendMessageAsync(msg);
                return true;
            }

            Logger.Debug("Return false");
            return false;
        }

        public static void InitializeMentions()
        {
            new Mention()
            {
                Prefix = "",
                Suffix = ".",
                Message = "%MENTION%, веточкой тык.",
                XP = 0
            };


            new Mention()
            {
                Prefix = "",
                Suffix = "!!!",
                Message = "%MENTION%, бревном хуяк!!!",
                XP = 60
            };


            new Mention()
            {
                Prefix = "",
                Suffix = "!",
                Message = "%MENTION%, палкой пиздык!",
                XP = 20
            };


            new Mention()
            {
                Prefix = "-",
                Suffix = "",
                Message = "%MENTION%, деревом еблысь!!11!1!1111",
                XP = 120
            };
        }
    }
}
