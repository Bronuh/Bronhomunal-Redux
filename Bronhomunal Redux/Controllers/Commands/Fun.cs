using Bronuh.Libs;
using Bronuh.Modules;
using Bronuh.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Controllers.Commands
{
	class Fun : ICommands
	{
		public void InitializeCommands()
		{


			CommandsController.AddCommand("waste", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				string respond = Wastificator.Wastificate(text.Replace(parts[0] + " ", ""));
				await m.RespondAsync(respond);
			})
			.AddAlias("потратить").AddAlias("потрачено")
			.SetOp(true)
			.SetDescription("Заставляет бота потратить что-то");


			CommandsController.AddCommand("infa", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				string respond = args + "\nИнфа: " + Math.Round(Infa.CheckInfo(text).Value, 0) + "%";
				await m.RespondAsync(respond);
			})
			.AddAlias("info").AddAlias("инфа")
			.SetDescription("Измеряет запрошенную инфу");
		}
	}
}
