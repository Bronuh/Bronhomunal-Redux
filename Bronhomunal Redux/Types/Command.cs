using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace Bronuh.Types
{

	public delegate Task CommandAction(ChatMessage msg);



	public class Command
	{
		public string Name { get; private set; }
		public string Description { get; private set; } = "нет описания";
		public string Usage { get; private set; } = "нет примера использования";
		public List<string> Aliases { get; private set; } = new List<string>();

		private readonly CommandAction _action;

		public int Rank { get; private set; } = 0;

		public bool OpOnly { get; private set; } = false;



		public Command() { }
		public Command(string name, CommandAction action) {
			Name = name;
			_action = action;
		}

		public string GetInfo()
		{
			string info = "";
			string aliases = "";
			foreach (string alias in Aliases)
			{
				aliases += alias+" ";
			}

			info += "Команда: "+Settings.Sign+Name+"\n";
			info += "Аналоги: "+aliases;
			info += "Использование: "+Usage+"\n";
			info += "Описание: "+Description+"\n";
			info += "Требуемый ранг: "+Rank+"\n";
			info += "Только для админов: "+OpOnly;

			return info;
		}

		public Command SetDescription(String description)
		{
			Description = description;
			return this;
		}

		public Command SetUsage(String usage)
		{
			Usage = usage;
			return this;
		}

		public Command SetRank(int rank)
		{
			Rank = rank;
			return this;
		}


		public Command SetOp(bool op)
		{
			OpOnly = op;
			return this;
		}


		public Command AddAlias(string alias)
		{
			Aliases.Add(alias);
			return this;
		}


		private bool CheckCommand(string text)
		{
			string command = text.Split(' ')[0];
			
			if (command.ToLower()==Name.ToLower())
			{
				return true;
			}
			else
			{
				Logger.Debug("Имя команды не найдено, поиск псевдонимов");
				foreach (string alias in Aliases)
				{
					Logger.Debug("Псевдоним: " + alias);
					if (command.ToLower() == alias.ToLower())
					{
						return true;
					}
				}
			}

			return false;
		}


		public async Task<bool> TryExecute(ChatMessage message)
		{
			string text = message.Text;
			Member author = message.Author;

			text = text.Substring(Settings.Sign.Length, text.Length-1);

			Logger.Debug("Попытка выполнения "+ Settings.Sign + Name +" ("+text+")");
			if (CheckCommand(text))
			{
				Logger.Debug("Обнаружена команда "+ Name);
				if (author.IsOP||author.Rank>=Rank)
				{
					if (OpOnly && !author.IsOP)
					{
						Logger.Warning("Команда только для операторов");
						return true;
					}

					await _action(message);
				}
				
				return true;
			}
			return false;
		}

	}
}
