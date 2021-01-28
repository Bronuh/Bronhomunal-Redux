﻿using Bronuh.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Controllers.Commands
{
	class Info : ICommands
	{
		public void InitializeCommands()
		{
			CommandsController.AddCommand("commands", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				int maxCommandsPerMessage = 5;
				int cmdInMessage = 0;

				string respond = "Список команд: \n\n";
				if (parts.Length > 1)
				{
					respond = "Список команд с тегом "+parts[1]+": \n\n";
				}

				foreach (Command command in CommandsController.Commands)
				{
					Logger.Debug("Команда "+command.Name);
					if (m.Author.IsOP || userRank >= command.Rank)
					{
						
						if (cmdInMessage >= maxCommandsPerMessage)
						{
							Logger.Debug("Слишком много команд. Новое сообщение...");
							await m.RespondPersonalAsync(respond);
							cmdInMessage = 0;
							respond = "\n";
						}
						if (parts.Length > 1)
						{
							Logger.Debug("Поиск по тегу "+parts[1] + " среди тегов "+command.Tags.ToLine());
							if (command.HasTag(parts[1]))
							{
								Logger.Debug("Тег "+parts[1]+" имется");
								respond += command.GetInfo() + "\n\n";
								cmdInMessage++;
							}
						}
						else
						{
							respond += command.GetInfo() + "\n\n";
							cmdInMessage++;
						}
						
						
					}
				}

				await m.RespondPersonalAsync(respond);
			})
			.AddAlias("команды").AddAlias("help").AddAlias("хелп").AddAlias("помощь")
			.SetDescription("Выводит список доступных команд")
			.SetUsage(Settings.Sign + "commands [tag]")
			.AddTag("help")
			.AddTag("info");



			CommandsController.AddCommand("whois", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = null;

				if (parts.Length > 1)
				{
					target = MembersController.FindMember(parts[1]);
				}
				else if (parts.Length == 1)
				{
					target = m.Author;
				}

				string respond = target?.GetInfo() ?? "";
				await m.RespondAsync(respond);
			})
			.AddAlias("who").AddAlias("кто")
			.SetUsage(Settings.Sign + "whois [username]")
			.SetDescription("Выводит информацию о пользователе")
			.AddTag("misc")
			.AddTag("info");



			CommandsController.AddCommand("commandtags", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				List<string> tags = new List<string>();
				CommandsController.Commands.ForEach(command=> {
					command.Tags.ForEach(tag=> {
						if (!tags.Contains(tag))
							tags.Add(tag);
					});
				});

				string respond = "Тэги команд:\n";
				foreach (string tag in tags)
					respond += tag + (tag == tags[^1] ? "" : ", ");

				
				await m.RespondAsync(respond);
			})
			.AddAlias("tags").AddAlias("тэги")
			.SetUsage(Settings.Sign + "commandtags")
			.SetDescription("Выводит известные теги для команд, для использования с !commands [tag]")
			.AddTag("help")
			.AddTag("info");
		}
	}
}
