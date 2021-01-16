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
		public static List<Command> Commands = new List<Command>();

		public static void AddCommand(string name, CommandAction action)
		{
			Commands.Add(new Command(name, action));
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
