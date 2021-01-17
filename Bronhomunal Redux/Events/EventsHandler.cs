using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Bronuh.Types;
using Bronuh.Controllers;

namespace Bronuh.Events
{
	public static class EventsHandler
	{
		
		public static async Task HandleEvent(EventArgs e)
		{
			if (e is MessageCreateEventArgs args)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await MessageCreateEventHandler(args);
			}
			else
			{
				Logger.Warning("Нет обработчика для события "+e.GetType().Name);
			}
		}

		private static async Task MessageCreateEventHandler(MessageCreateEventArgs e)
		{
			e.Message.Author.ToMember().XP++;
			Logger.Log($"{e.Author.ToMember().DisplayName}: {e.Message.Content}");

			if (e.Message.Content.StartsWith(Settings.Sign))
			{
				await CommandsController.TryExecuteCommand(e);
			}
			else
			{
				await MentionsController.Execute(e);
			}
			
		}
	}
}
