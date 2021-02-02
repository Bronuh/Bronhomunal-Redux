using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus.Entities;

namespace Bronuh.Logic
{
	public class Worker
	{
		public static void EverySecond(object state)
		{
			Logger.Debug("Checking voice time...");
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
				foreach (DiscordRole role in Bot.Guild.Roles.Values)
				{
					if (role.Name == "Minecraft")
					{
						minecraft = role.Mention;
					}
				}

				MineStat ms = new MineStat("abro.tech", 25565);
				if (ms.ServerUp)
				{
					if (!Settings.ServerStatus)
					{
						Bot.GamesChannel.SendMessageAsync(":white_check_mark: " + minecraft + " Сервер abro.tech **ВКЛЮЧЕН**").GetAwaiter().GetResult();
					}
				}
				else
				{
					if (Settings.ServerStatus)
					{
						Bot.GamesChannel.SendMessageAsync(":no_entry: " + minecraft + " Сервер abro.tech **ВЫКЛЮЧЕН**").GetAwaiter().GetResult();
					}
				}
				Settings.ServerStatus = ms.ServerUp;
				Logger.Debug("Server status " + ms.ServerUp);
			}
		}

		private static void CheckVoiceTime()
		{
			foreach (Member member in MembersController.Members)
			{
				if (member.GetVoiceTime() >= 60 * 1000)
				{
					member.GiveAchievement("voice1").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 600 * 1000)
				{
					member.GiveAchievement("voice2").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600 * 1000)
				{
					member.GiveAchievement("voice3").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600 * 6 * 1000)
				{
					member.GiveAchievement("voice4").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600 * 12 * 1000)
				{
					member.GiveAchievement("voice5").GetAwaiter().GetResult();
				}
			}
		}
	}
}
