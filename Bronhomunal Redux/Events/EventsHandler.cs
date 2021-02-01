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
				return;
			}

			else if (e is GuildMemberAddEventArgs guildMemberAddEventArgs)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await GuildMemberAddEventHandler(guildMemberAddEventArgs);
				return;
			}

			else if (e is GuildMemberRemoveEventArgs guildMemberRemoveEventArgs)
			{
				Logger.Debug("Обработка события " + e.GetType().Name + "...");
				await GuildMemberRemoveEventHandler(guildMemberRemoveEventArgs);
				return;
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
				if (!e.Channel.IsPrivate)
				{
					if (e.Channel.Name.StartsWith("bot"))
					{
						await sender.GiveAchievement("ai");
					}

					if (e.Channel.Name.StartsWith("asociality"))
					{
						await sender.GiveAchievement("btard");
					}

					if (e.Channel.Name.StartsWith("dev"))
					{
						await sender.GiveAchievement("coolhacker");
					}
				}
				

				Logger.Log($"{e.Author.ToMember().DisplayName}: {e.Message.Content}");

				if (!e.Channel.IsPrivate||sender.IsOwner())
				{
					if (e.Message.Content.StartsWith(Program.Prefix + Settings.Sign))
					{
						await CommandsController.TryExecuteCommand(e);
					}
					else
					{
						await MentionsController.Execute(e);
					}
				}
				else
				{
					await e.Message.RespondAsync("Сюда низя писатб");
				}
			}
		}


		
		private static async Task GuildMemberAddEventHandler(GuildMemberAddEventArgs e)
		{
			await Bot.OutpostChannel.SendMessageAsync(e.Member.Username+"#"+e.Member.Discriminator+" зашел на сервер");
			await MembersController.HardUpdate();
		}


		private static async Task GuildMemberRemoveEventHandler(GuildMemberRemoveEventArgs e)
		{
			await Bot.OutpostChannel.SendMessageAsync(e.Member.Username + "#" + e.Member.Discriminator + " свалил с сервера." +
				" Но я тебя запомнил...");
			await MembersController.HardUpdate();
		}
	}
}
