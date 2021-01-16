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
			if (e is MessageCreateEventArgs)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await MessageCreateEventHandler((MessageCreateEventArgs)e);
			}
			else
			{
				Logger.Warning("Нет обработчика для события "+e.GetType().Name);
			}
		}

		private static async Task MessageCreateEventHandler(MessageCreateEventArgs e)
		{
			
			if (e.Message.Content.StartsWith(Settings.GetSign()))
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
