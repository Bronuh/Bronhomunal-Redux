using Bronuh.Types;
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

				foreach (Command command in CommandsController.Commands)
				{
					if (m.Author.IsOP || userRank >= command.Rank)
					{
						if (cmdInMessage >= maxCommandsPerMessage)
						{
							await m.RespondPersonalAsync(respond);
							cmdInMessage = 0;
							respond = "\n";
						}
						respond += command.GetInfo() + "\n\n";
						cmdInMessage++;
					}
				}

				await m.RespondPersonalAsync(respond);
			})
			.AddAlias("команды")
			.SetDescription("Выводит список доступных команд")
			.SetUsage(Settings.Sign + "commands");



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
			.SetDescription("Выводит информацию о пользователе");
		}
	}
}
