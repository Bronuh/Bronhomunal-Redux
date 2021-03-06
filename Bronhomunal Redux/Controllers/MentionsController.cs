﻿using Bronuh.Types;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bronuh.Controllers
{
	public class MentionsController
	{
		private static bool _initialized = false;
		public static List<Mention> Mentions { get; private set; } = new List<Mention>();

		/// <summary>
		/// Попытка обработать сообщение как упоминание
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static async Task<bool> Execute(MessageCreateEventArgs e)
		{
			if (!_initialized)
			{
				InitializeMentions();
				_initialized = true;
			}

			Logger.Debug("Сообщение передано в MentionsController.Execute(s)");
			string text = e.Message.Content;
			string[] words = text.Split(' ');
			string msg = "";
			bool mentioned = true;

			foreach (string word in words)
			{
				if (mentioned)
				{
					foreach (Mention mention in Mentions)
					{
						if (mention.Match(word))
						{
							Member author = MembersController.FindMember(e.Author.Id);
							if (author.CanUse(mention))
							{
								Logger.Debug("Найдено соответствие (" + mention.GetClearText(word) + ")");
								if (mention.GetClearText(word).Length >= 3)
								{
									Member member = MembersController.FindMember(mention.GetClearText(word));
									if (member != null)
									{
										Logger.Debug("Найденный пользователь: " + member.Username);
										msg += mention.Message.Replace("%MENTION%", "<@!" + member.Id + ">") + "\n";
										mention.CustomAction(author, member);
										Member.OnUsedMention(author, new Events.MemberUsedMentionEventArgs()
										{
											Author = author,
											Target = member,
											Mention = mention
										});

										if (member.Source.IsBot)
										{
											await author.GiveAchievement("why");
										}
										if (author.Statistics.StickPokes > 0
											&& author.Statistics.StickHits > 0
											&& author.Statistics.LogHits > 0
											&& author.Statistics.TreeHits > 0)
										{
											await author.GiveAchievement("overwhelming");
										}
									}
								}
							}
							else
							{
								msg += "Слишком низкий ранг для этого действия (" + author.Rank + "/" + mention.Rank + ")\n";
							}

							mentioned = true;
							break;
						}
						Logger.Debug("mentioned = false");
						mentioned = false;
					}
				}
				else
				{
					break;
				}

			}

			if (msg != "")
			{
				await Bot.SendMessageAsync(msg);
				return true;
			}

			Logger.Debug("Return false");
			return false;
		}

		private static void InitializeMentions()
		{
			new Mention()
			{
				Prefix = "",
				Suffix = ".",
				Message = "%MENTION%, веточкой тык.",
				Rank = 1,
				CustomAction = async (sender, target) =>
				{
					sender.Statistics.StickPokes++;
					target.Statistics.PokedByStick++;
					if (sender.Statistics.StickPokes >= AchievementsController.StickPoke.CustomValue)
					{
						await sender.GiveAchievement("stickpoke");
					}
				}
			};

			new Mention()
			{
				Prefix = "",
				Suffix = "!!!",
				Message = "%MENTION%, бревном хуяк!!!",
				Rank = 2,
				CustomAction = async (sender, target) =>
				{
					sender.Statistics.LogHits++;
					target.Statistics.HitByLog++;

					if (sender.Statistics.LogHits >= AchievementsController.LogHit.CustomValue)
					{
						await sender.GiveAchievement("loghit");
					}

					if (sender.Statistics.HitByLog >= AchievementsController.Logged.CustomValue)
					{
						await sender.GiveAchievement("logged");
					}
				}
			};

			new Mention()
			{
				Prefix = "",
				Suffix = "!",
				Message = "%MENTION%, палкой пиздык!",
				Rank = 1,
				CustomAction = async (sender, target) =>
				{
					sender.Statistics.StickHits++;
					target.Statistics.HitByStick++;
					if (sender.Statistics.StickHits >= AchievementsController.StickHit.CustomValue)
					{
						await sender.GiveAchievement("stickhit");
					}
				}
			};

			new Mention()
			{
				Prefix = "-",
				Suffix = "",
				Message = "%MENTION%, деревом еблысь!!11!1!1111",
				Rank = 3,
				CustomAction = async (sender, target) =>
				{
					sender.Statistics.TreeHits++;
					target.Statistics.HitByTree++;

					if (sender.Statistics.TreeHits >= AchievementsController.TreeHit.CustomValue)
					{
						await sender.GiveAchievement("treehit");
					}

					if (sender.Statistics.HitByTree >= AchievementsController.Treed.CustomValue)
					{
						await sender.GiveAchievement("treed");
					}

					if (target.IsOp())
					{
						await sender.GiveAchievement("riot");
					}
				}
			};
		}
	}
}
