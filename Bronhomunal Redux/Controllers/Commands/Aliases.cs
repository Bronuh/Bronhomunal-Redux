using Bronuh.Types;

namespace Bronuh.Controllers.Commands
{
	class Aliases : ICommands
	{
		public void InitializeCommands()
		{
			CommandsController.AddCommand("alias", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				Member target = MembersController.FindMember(parts[1]);
				Alias alias = AliasesController.AddAlias(parts[2], target);
				await m.RespondAsync($"Запомнил: {target.DisplayName} -> {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Запоминает псевдоним указанного пользователя")
			.SetUsage("<command> username alias")
			.AddAlias("запомни").AddAlias("алиас")
			.AddTag("misc");

			CommandsController.AddCommand("forget", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');

				Alias alias = AliasesController.RemoveAlias(parts[1]);

				await m.RespondAsync($"Забыл: {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Забывает псевдоним указанного пользователя")
			.SetUsage("<command> alias")
			.AddAlias("забудь")
			.AddTag("misc");
		}
	}
}
