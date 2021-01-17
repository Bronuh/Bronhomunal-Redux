using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Bronuh.Types;
using System.Threading.Tasks;

namespace Bronuh
{
	public static class CommandsController
	{
		public static List<Command> Commands { get; private set; } = new List<Command>();
		private static bool _initialized = false;


		public static Command AddCommand(string name, CommandAction action)
		{
			Command command = new Command(name, action);
			Commands.Add(command);
			return command;
		}


		public static async Task<bool> TryExecuteCommand(MessageCreateEventArgs e)
		{
			if (!_initialized)
				InitializeCommands();

			bool executed = false;
			foreach (Command command in Commands)
			{
				executed = await command.TryExecute(new ChatMessage(e.Message));
			}

			return executed;
		}

		public static async Task<bool> TryExecuteConsoleCommand(string cmd)
		{
			if (!_initialized)
				InitializeCommands();

			bool executed = false;
			foreach (Command command in Commands)
			{
				executed = await command.TryExecute(new ChatMessage(Settings.Sign+cmd));
			}

			return executed;
		}


		private static void InitializeCommands()
		{
			AddCommand("alias",async (m) => {
				string text = m.Text;
				string[] parts = text.Split(' ');
				Member target = MembersController.FindMember(parts[1]);
				Alias alias = AliasesController.AddAlias(parts[2],target);
				await m.RespondAsync($"Запомнил: {target.DisplayName} -> {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Запоминает псевдоним указанного пользователя")
			.SetUsage(Settings.Sign+"alias user alias")
			.AddAlias("запомни").AddAlias("алиас");


			AddCommand("forget", async (m) => {
				string text = m.Text;
				string[] parts = text.Split(' ');

				Alias alias = AliasesController.RemoveAlias(parts[1]);

				await m.RespondAsync($"Забыл: {alias.Name}");
			})
			.SetRank(1)
			.SetDescription("Забывает псевдоним указанного пользователя")
			.SetUsage(Settings.Sign+"forget alias")
			.AddAlias("забудь");


			AddCommand("commands", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				string respond = "Список команд: \n\n";

				foreach (Command command in Commands)
				{
					if (m.Author.IsOP || userRank >= command.Rank)
					{
						respond += command.GetInfo() + "\n\n";
					}
				}

				await m.RespondAsync(respond);
			})
			.AddAlias("команды")
			.SetDescription("Выводит список доступных команд")
			.SetUsage(Settings.Sign+"commands");

			_initialized = true;
		}
	}
}
