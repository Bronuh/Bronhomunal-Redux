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
		public string Name;
		public string Description;
		private CommandAction _action;
		public int Rank = 0;
		public List<string> Aliases = new List<string>();

		public Command() { }
		public Command(string name, CommandAction action) {
			Name = name;
			_action = action;
		}

		public Command SetDescription(String description)
		{
			Description = description;
			return this;
		}

		public Command SetRank(int rank)
		{
			Rank = rank;
			return this;
		}

		public Command AddAlias(string alias)
		{
			Aliases.Add(alias);
			return this;
		}

		public async Task<bool> TryExecute(ChatMessage message)
		{
			string text = message.Text;
			Member author = message.Author;

			text = text.Substring(Settings.GetSign().Length, text.Length-1);

			Logger.Debug("Попытка выполнения "+ Settings.GetSign() + Name +" ("+text+")");
			if (text.StartsWith(Name))
			{
				Logger.Debug("Обнаружена команда "+ Name);
				if (author.IsOP||author.Rank>=Rank)
				{
					await _action(message);
				}
				
				return true;
			}
			return false;
		}

	}
}
