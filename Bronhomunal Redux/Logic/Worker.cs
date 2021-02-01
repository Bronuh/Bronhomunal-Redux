using Bronuh.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Logic
{
	public class Worker
	{
		public static void Work(object state)
		{
			//Logger.Log("Checking voice time...");
			if (Bot.Ready)
			{
				
				CheckVoiceTime();
			}
			
		}

		private static void CheckVoiceTime()
		{
			foreach(Member member in MembersController.Members)
			{
				if (member.GetVoiceTime()>=60*1000)
				{
					member.GiveAchievement("voice1").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 600*1000)
				{
					member.GiveAchievement("voice2").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600*1000)
				{
					member.GiveAchievement("voice3").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600*6*1000)
				{
					member.GiveAchievement("voice4").GetAwaiter().GetResult();
				}

				if (member.GetVoiceTime() >= 3600 * 12* 1000)
				{
					member.GiveAchievement("voice5").GetAwaiter().GetResult();
				}
			}
		}
	}
}
