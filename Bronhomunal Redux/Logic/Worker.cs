using Bronuh.Controllers;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bronuh.Logic
{
	public class Worker
	{
		public static void EverySecond(object state)
		{
			Logger.Debug("Every 5 seconds executed");
			if (Bot.Ready)
			{
				CheckVoiceTime();
			}
		}

		public static void Every30Sec(object state)
		{
			Logger.Debug("Checking server status...");
			Logger.Log("Проверка слушателей серверов...");
			ServerChecker.Checkers.EachAsync(c=>c.Check());

			if (Bot.Ready)
			{
				string minecraft = "";
				var keyMember = MembersController.FindMember("Key_J");
				string key = keyMember.Source.Mention;
				foreach (DiscordRole role in Bot.Guild.Roles.Values)
				{
					if (role.Name.ToLower() == "minecraft-event")
					{
						minecraft = role.Mention;
					}
				}

			}
		}


		public static void Every5Min(object state)
		{
			Logger.Debug("Every 5 min");
			Program.SaveAll();
		}


		private static void CheckVoiceTime()
		{
			Logger.Debug("Checking max and total voice time");
			foreach (Member member in MembersController.Members)
			{
				member.Statistics.MaxVoiceSessionTime.value = member.GetMaxVoiceTime();

				for (int i = 1; i<=5; i++)
				{
					if (member.GetMaxVoiceTime() >= AchievementsController.Find("voice"+i).CustomValue)
					{
						member.GiveAchievement("voice"+i).GetAwaiter().GetResult();
					}
				}

				for (int i = 1; i <= 3; i++)
				{
					if (member.GetVoiceTime() >= AchievementsController.Find("totalvoice" + i).CustomValue * 1000 * 60 * 60)
					{
						member.GiveAchievement("totalvoice" + i).GetAwaiter().GetResult();
					}
				}
			}
		}
	}
}
