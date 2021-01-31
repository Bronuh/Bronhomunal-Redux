using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Bronuh.Types;
using System.Threading.Tasks;
using Bronuh.Libs;
using Bronuh.Modules;
using DSharpPlus.Entities;
using System.Reflection;
using System.Linq;

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
			if (!_initialized)
				InitializeCommands();

			bool executed;
			foreach (Command command in Commands)
			{
				executed = await command.TryExecute(new ChatMessage(Settings.Sign+cmd));
				if (executed)
				{
					return true;
				}
			}

			return false;
		}


		
		private static void InitializeCommands()
		{

			/*
			AddCommand("",async (m) => 
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				Member user = m.Author;
				int userRank = user.Rank;

				string respond = "Ответ: ";
				await m.RespondAsync(respond);
			});
			*/
			// 

			InterfaceExecutor.Execute(typeof(ICommands), "InitializeCommands");

			_initialized = true;

			
		}
	}
}
