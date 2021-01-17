using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bronuh.Events;

namespace Bronuh
{
	static class Bot
	{
		public static string Token;
		public static DiscordClient Discord { get; private set; }
		public static bool Ready = false;
		public static IReadOnlyList<DiscordChannel> Channels = new List<DiscordChannel>();
		public static DiscordChannel BotChannel, LastChannel, OutpostChannel;
		public static DiscordGuild Guild;
		private static readonly ulong RequiredGuildId = 308653152054280195;
		private static readonly ulong BotID = 696952183572267028;
		public static List<DiscordMember> DiscordMembers = new List<DiscordMember>();



		public static void Initialize(string token)
		{
			Token = token;
			Logger.Log("Запуск асинхронного метода...");
			MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static async Task MainAsync()
		{
			/// Подклоючение бота
			Logger.Log("Подключение Дискорд-клиента...");
			Discord = new DiscordClient(new DiscordConfiguration
			{
				Token = Token,
				TokenType = TokenType.Bot,
			});



			Logger.Log("Обработчик события Ready");
			Discord.Ready += async (Discord, e) =>
			{
				Logger.Log("Поиск целевого сервера...");
				if (Discord.Guilds.Count == 1)
				{
					Guild = new List<DiscordGuild>(Discord.Guilds.Values)[0];
					Logger.Log("Сервер: "+Guild.Id);
				}
				else
				{
					Logger.Log("Серверов в списке больше чем 1");
					foreach (KeyValuePair<ulong, DiscordGuild> KV in Discord.Guilds)
					{
						DiscordGuild currentGuild = KV.Value;

						if (currentGuild.Id == RequiredGuildId)
						{
							Logger.Success("Нужный сервер найден!");
							Guild = KV.Value;
							Logger.Log("Сервер: " + Guild.Name);
							break;
						}
					}
				}

				if (Guild != null)
				{
					Channels = await Guild.GetChannelsAsync();


					Logger.Log("Поиск канала для бота...");
					foreach (DiscordChannel currentChannel in Channels)
					{
						Logger.Debug("Канал: " + currentChannel.Name);
						if (currentChannel.Name.ToLower() == "bot" || currentChannel.Name.ToLower() == "bots")
						{
							Logger.Success("Найден канал для ботов!");
							BotChannel = currentChannel;
							break;
						}
					}


					Logger.Log("Поиск канала для прибывающих...");
					foreach (DiscordChannel currentChannel in Channels)
					{
						Logger.Debug("Канал: " + currentChannel.Name);
						if (currentChannel.Name.ToLower() == "outpost")
						{
							Logger.Success("Найден канал для прибывающих!");
							OutpostChannel = currentChannel;
							break;
						}
					}
				}
				Logger.Log("Получение списка участников...");


				IReadOnlyCollection<DiscordMember> members = new List<DiscordMember>();
				try
				{
					members = await Guild.GetAllMembersAsync();
				}catch(Exception ex)
				{
					Logger.Error("Дерьмо случается!");
					Logger.Error(ex.Message);
				}
				Logger.Log("Связывание списка участников...");
				foreach (DiscordMember kv in members)
				{
					DiscordMembers.Add(kv);
				}
				MembersController.LinkDiscordMembers(DiscordMembers);

				Ready = true;

				Logger.Log("Получен список участников");
			};


			Discord.MessageCreated += async (Discord, e) => { 
				if (e.Author.Id != BotID) { 
					LastChannel = e.Channel; 
					await EventsHandler.HandleEvent(e); 
				} 
			};

			await Discord.ConnectAsync();
			await Task.Delay(-1);
		}

		public static void SendMessage(String msg)
		{
			SendMessageAsync(msg).GetAwaiter().GetResult();
		}


		/// <summary>
		/// Асинхронный метод отправки сообщения
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static async Task SendMessageAsync(String msg)
		{
			if (Ready)
			{
				if (LastChannel != null)
				{
					await LastChannel.SendMessageAsync(msg);
				}
				else
				{
					Logger.Warning("Нет последнего канала!1!");
					if (BotChannel != null)
					{
						await BotChannel.SendMessageAsync(msg);
					}
					else
					{
						Logger.Warning("Невозможно отправить сообщение! Нет даже канала для ботов!");
					}
				}
			}
			else
			{
				Logger.Error("Инициализация не закончена!");
			}
		}
	}
}
