using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Bronuh.Types;
using System.Threading.Tasks;
using Bronuh.Libs;
using Bronuh.Modules;

namespace Bronuh
{
	public static class CommandsController
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

			bool executed = false;
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

			bool executed = false;
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
				int maxCommandsPerMessage = 5;
				int cmdInMessage = 0;

				string respond = "Список команд: \n\n";

				foreach (Command command in Commands)
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
			.SetUsage(Settings.Sign+"commands");


			AddCommand("debug", async (m) =>
			{
				Settings.DEBUG = !Settings.DEBUG;

				string respond = "Отображение сообщений отладки: "+Settings.DEBUG;

				await m.RespondAsync(respond);
			})
			.SetOp(true)
			.SetDescription("Включает/выключает вывод отладочных сообщений в консоль")
			.SetUsage(Settings.Sign+"debug");


			AddCommand("token", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string token = parts[1];
				Settings.SetToken(token);

				await CommandsController.TryExecuteConsoleCommand("kill");

				string respond = "Установлен токен: "+token;
				await m.RespondAsync(respond);
			});


			AddCommand("whois", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = null;

				if (parts.Length>1)
				{
					target = MembersController.FindMember(parts[1]);
				}
				else if (parts.Length==1)
				{
					target = m.Author;
				}

				string respond = target?.GetInfo()??"";
				await m.RespondAsync(respond);
			})
			.AddAlias("who").AddAlias("кто")
			.SetUsage(Settings.Sign + "whois [username]")
			.SetDescription("Выводит информацию о пользователе");



			AddCommand("say", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				string respond = text.Replace(parts[0] + " ", "");
				await m.RespondAsync(respond);
			})
			.AddAlias("скажи")
			.SetOp(true)
			.SetDescription("Заставляет бота сказать что-то");


			AddCommand("waste", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;

				Member target = m.Author;

				string respond = Wastificator.Wastificate(text.Replace(Settings.Sign + parts[0] + " ", ""));
				await m.RespondAsync(respond);
			})
			.AddAlias("потратить")
			.SetOp(true)
			.SetDescription("Заставляет бота потратить что-то");


			AddCommand("infa", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				string respond = args+"\nИнфа: " + Math.Round(Infa.CheckInfo(text).Value, 0) + "%";
				await m.RespondAsync(respond);
			})
			.AddAlias("info").AddAlias("инфа")
			.SetDescription("Измеряет запрошенную инфу");

			/*
			AddCommand("",async (m) => 
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				string args = text.Replace(parts[0] + " ", "");
				int userRank = m.Author.Rank;

				string respond = "Ответ: ";
				await m.RespondAsync(respond);
			});
			*/
			// 
			_initialized = true;
		}
	}
}
