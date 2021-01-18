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
			if (e is MessageCreateEventArgs messageCreateEventArgs)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await MessageCreateEventHandler(messageCreateEventArgs);
			}

			if (e is GuildMemberAddEventArgs guildMemberAddEventArgs)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await GuildMemberAddEventHandler(guildMemberAddEventArgs);
			}
			else
			{
				Logger.Warning("Нет обработчика для события "+e.GetType().Name);
			}
		}


		private static async Task MessageCreateEventHandler(MessageCreateEventArgs e)
		{
			
			Member sender = e.Message.Author.ToMember();
			if (!sender.IsBronomunal())
			{
				sender.LastMessage = new ChatMessage(e.Message);
				await sender.AddXPAsync(1);

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


		private static async Task GuildMemberAddEventHandler(GuildMemberAddEventArgs e)
		{

		}
	}
}
