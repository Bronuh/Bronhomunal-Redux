using Bronuh.Events;
using Bronuh.Modules;
using Bronuh.Types;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bronuh
{
	static class Bot
	{
		public static string Token;
		public static DiscordClient Discord { get; private set; }
		public static bool Ready = false;
		public static IReadOnlyList<DiscordChannel> Channels = new List<DiscordChannel>();
		public static DiscordChannel BotChannel, LastChannel, OutpostChannel, GamesChannel;
		public static DiscordGuild Guild;
		private static readonly ulong RequiredGuildId = 308653152054280195;
		private static readonly ulong BotID = 696952183572267028;
		public static List<DiscordMember> DiscordMembers = new List<DiscordMember>();

		public static void Initialize(string token)
		{
			Token = token;
			Logger.Log("Запуск асинхронного метода...");
			try
			{
				MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);
			}
		}

		public static async Task MainAsync()
		{
			/// Подклоючение бота
			Logger.Log("Подключение Дискорд-клиента...");
			Discord = new DiscordClient(new DiscordConfiguration
			{
				Token = Token,
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.All
			});

			Logger.Debug("Обработчик события Ready");
			Discord.Ready += async (Discord, e) =>
			{
				Logger.Log("Поиск целевого сервера...");
				if (Discord.Guilds.Count == 1)
				{
					Guild = new List<DiscordGuild>(Discord.Guilds.Values)[0];
					Logger.Debug("Сервер: " + Guild.Id);
				}
				else
				{
					Logger.Debug("Серверов в списке больше чем 1");
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

					Logger.Log("Поиск канала для игор...");
					foreach (DiscordChannel currentChannel in Channels)
					{
						Logger.Debug("Канал: " + currentChannel.Name);
						if (currentChannel.Name.ToLower() == "gaems")
						{
							Logger.Success("Найден канал для игор!");
							GamesChannel = currentChannel;
							break;
						}
					}
				}
				Logger.Log("Получение списка участников...");

				IReadOnlyCollection<DiscordMember> members = new List<DiscordMember>();
				try
				{
					members = await Guild.GetAllMembersAsync();
				}
				catch (Exception ex)
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
				Logger.Success("Бот запущен");
				await SendMessageAsync("(" + DateTime.Now.ToLongTimeString() + ") Запуск № " + Settings.LaunchCount);
			};

			Discord.MessageCreated += async (Discord, e) =>
			{
				if (e.Author.Id != BotID)
				{
					if (!e.Channel.IsPrivate)
					{
						LastChannel = e.Channel;
					}
					await EventsHandler.HandleEvent(e);
				}
			};

			Discord.VoiceStateUpdated += async (discord, e) =>
			{
				if (e.Channel != null)
				{
					foreach (DiscordMember member in e.Channel.Users)
					{
						if (!member.ToMember().IsInVoice)
						{
							var mamba = member.ToMember();
							mamba.JoinVoice();
							Member.OnJoinedVoice(mamba, new MemberJoinedVoiceEventArgs());
						}
					}

					var list = new List<DiscordMember>(e.Channel.Users);

					if (e.Channel.Name == "Хотсазаляция")
					{
						foreach (DiscordMember member in list)
						{
							if (member.Id == 312231263471796234)
							{
								foreach (DiscordMember member2 in list)
								{
									await member2.ToMember().GiveAchievement("zoologist");
								}
								break;
							}
						}
					}

					if (e.After.IsSelfDeafened)
					{
						await e.User.ToMember().GiveAchievement("stone");
					}

					if (e.User.Id == 263705631549161472)
					{
						if (!e.After.IsSelfMuted)
						{
							foreach (DiscordMember member2 in list)
							{
								await member2.ToMember().GiveAchievement("ascension");
							}
						}
					}

					foreach (DiscordMember member in list)
					{
						if (member.Id == 263705631549161472)
						{
							foreach (DiscordMember member2 in list)
							{
								await member2.ToMember().GiveAchievement("bronuh");
							}
							break;
						}
					}

					if (list.Count == 1)
					{
						foreach (DiscordMember member in list)
						{
							await member.ToMember().GiveAchievement("alone");
						}
					}

					if (list.Count >= 6)
					{
						foreach (DiscordMember member in list)
						{
							await member.ToMember().GiveAchievement("party");
						}
					}

					if (list.Count >= 8)
					{
						foreach (DiscordMember member in list)
						{
							await member.ToMember().GiveAchievement("crowd");
						}
					}

					if (list.Count >= 10)
					{
						foreach (DiscordMember member in list)
						{
							await member.ToMember().GiveAchievement("zerg");

						}
					}
				}
				else
				{
					if (e.Before.Channel != null)
					{
						foreach (DiscordMember member in e.Before.Channel.Users)
						{
							if (!member.ToMember().IsInVoice)
							{
								var mamba = member.ToMember();
								mamba.JoinVoice();
								Member.OnJoinedVoice(mamba, new MemberJoinedVoiceEventArgs());
							}
						}
					}
					e.User.ToMember().LeaveVoice();
					Member.OnLeavedVoice(e.User.ToMember(), new MemberLeavedVoiceEventArgs());
				}
				var _chans = e.Guild.Channels;

				foreach (DiscordChannel chan in _chans.Values)
				{
					if (chan.Type == ChannelType.Voice)
					{
						var users = new List<DiscordMember>(chan.Users);
						foreach (DiscordMember member in users)
						{
							if (!member.ToMember().IsInVoice)
							{
								var mamba = member.ToMember();
								mamba.JoinVoice();
								Member.OnJoinedVoice(mamba, new MemberJoinedVoiceEventArgs());
							}
						}
					}
				}
			};

			Discord.GuildMemberAdded += async (Discord, e) => { await EventsHandler.HandleEvent(e); };

			await Discord.ConnectAsync();
			await Task.Delay(-1);
		}

		public static void SendMessage(String msg)
		{
			SendMessageAsync(msg + Program.Suffix).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Асинхронный метод отправки сообщения
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static async Task SendMessageAsync(string msg)
		{
			await SendMessageAsync(new DiscordMessageBuilder().WithContent(msg));
		}

		/// <summary>
		/// Асинхронный метод отправки сложного сообщения
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static async Task SendMessageAsync(DiscordMessageBuilder builder)
		{
			if (Ready)
			{
				builder.Content += Program.Suffix;
				if (LastChannel != null)
				{
					await LastChannel.SendMessageAsync(builder);
				}
				else
				{
					Logger.Warning("Нет последнего канала!1!");
					if (BotChannel != null)
					{
						LastChannel = BotChannel;
						await BotChannel.SendMessageAsync(builder);
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
