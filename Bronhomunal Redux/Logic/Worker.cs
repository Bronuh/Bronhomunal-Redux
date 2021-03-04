using Bronuh.Controllers;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;

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

				MineStat ms = new MineStat("abro.tech", 25565);
				if (ms.ServerUp)
				{
					if (!Settings.ServerStatus)
					{
						Settings.ServerStatus = true;
						keyMember.Source.SendMessageAsync(":white_check_mark: " + minecraft + " Сервер abro.tech **ВКЛЮЧЕН**\n" +
							"Подписаться на уведомления: !giverole minecraft-event").GetAwaiter().GetResult();
					}
				}
				else
				{
					MineStat ms2 = new MineStat("abro.tech", 25565);
					if (!ms2.ServerUp)
					{
						MineStat ms3 = new MineStat("abro.tech", 25565, 10);
						if (!ms3.ServerUp)
						{
							MineStat ms4 = new MineStat("abro.tech", 25565, 10);
							if (!ms4.ServerUp)
							{
								MineStat ms5 = new MineStat("abro.tech", 25565, 10);
								if (!ms5.ServerUp)
								{
									MineStat ms6 = new MineStat("abro.tech", 25565, 10);
									if (!ms6.ServerUp)
									{
										if (Settings.ServerStatus)
										{
											Settings.ServerStatus = false;
											keyMember.Source.SendMessageAsync(":no_entry: " + key + " Сервер abro.tech **ВЫКЛЮЧЕН**").GetAwaiter().GetResult();
										}
									}
								}
							}
						}
					}
				}
				
				Logger.Debug("Server status " + ms.ServerUp);
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
