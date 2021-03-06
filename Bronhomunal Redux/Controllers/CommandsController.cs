﻿using Bronuh.Types;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bronuh
{

	public static partial class CommandsController
	{
		public static List<Command> Commands { get; private set; } = new List<Command>();
		private static bool _initialized = false;

		/// <summary>
		/// Добавляет команду в общий список
		/// </summary>
		/// <param name="name">Сама команда</param>
		/// <param name="action">Делегат действия</param>
		/// <returns>Ссылка на добавленную команду</returns>
		public static Command AddCommand(string name, CommandAction action)
		{
			Command command = new Command(name, action);
			Commands.Add(command);
			return command;
		}

		/// <summary>
		/// Ищет команду по строке
		/// </summary>
		/// <param name="name">Сама команда</param>
		/// <param name="action">Делегат действия</param>
		/// <returns>Ссылка на добавленную команду</returns>
		public static Command FindCommand(string name)
		{
			Command command = null;

			foreach (var cmd in Commands)
			{
				if (cmd.CheckCommand(name))
				{
					command = cmd;
					break;
				}
			}

			return command;
		}

		/// <summary>
		/// Пробует выполнить команду, отправленную из дискорда
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static async Task<bool> TryExecuteCommand(MessageCreateEventArgs e)
		{
			if (!_initialized)
				InitializeCommands();

			bool executed;
			foreach (Command command in Commands)
			{
				executed = await command.TryExecute(new ChatMessage(e.Message));
				if (executed)
				{
					Member.OnExecutedCommand(e.Author.ToMember(), new Events.MemberExecutedCommandEventArgs(
						new ChatMessage(e.Message),
						command));
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Пробует выполнить консольную команду
		/// </summary>
		/// <param name="cmd">Текст команды</param>
		/// <returns></returns>
		public static async Task<bool> TryExecuteConsoleCommand(string cmd)
		{
			bool executed;
			foreach (Command command in Commands)
			{
				executed = await command.TryExecute(new ChatMessage(Program.Prefix + Settings.Sign + cmd));
				if (executed)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Запускает поиск и инициализацию всех команд. Содержит шаблон объявления команды в комментарии
		/// </summary>
		private static void InitializeCommands()
		{
			_initialized = true;
		}
	}
}
