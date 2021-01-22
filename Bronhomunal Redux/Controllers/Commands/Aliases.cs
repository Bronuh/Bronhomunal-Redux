using Bronuh.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Controllers.Commands
{
	class Aliases : ICommands
	{
		public void InitializeCommands()
		{
			CommandsController.AddCommand("alias", async (m) => {
				string text = m.Text;
				string[] parts = text.Split(' ');
				Member target = MembersController.FindMember(parts[1]);
				Alias alias = AliasesController.AddAlias(parts[2], target);
				await m.RespondAsync($"Запомнил: {target.DisplayName} -> {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Запоминает псевдоним указанного пользователя")
			.SetUsage(Settings.Sign + "alias user alias")
			.AddAlias("запомни").AddAlias("алиас");


			CommandsController.AddCommand("forget", async (m) => {
				string text = m.Text;
				string[] parts = text.Split(' ');

				Alias alias = AliasesController.RemoveAlias(parts[1]);

				await m.RespondAsync($"Забыл: {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Забывает псевдоним указанного пользователя")
			.SetUsage(Settings.Sign + "forget alias")
			.AddAlias("забудь");
		}
	}
}
