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

		public static Command AddCommand(string name, CommandAction action)
		{
			Command command = new Command(name, action);
			Commands.Add(command);
			return command;
		}


		public static async Task<bool> TryExecuteCommand(MessageCreateEventArgs e)
		{
			bool executed = false;
			foreach (Command command in CommandsController.Commands)
			{
				executed = await command.TryExecute(new ChatMessage(e.Message));
			}

			return executed;
		}

		public static async Task<bool> TryExecuteConsoleCommand(string cmd)
		{
			bool executed = false;
			foreach (Command command in CommandsController.Commands)
			{
				executed = await command.TryExecute(new ChatMessage(Settings.GetSign()+cmd));
			}

			return executed;
		}

	}
}
