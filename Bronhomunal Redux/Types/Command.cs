using Bronuh.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bronuh.Types
{
	public delegate Task CommandAction(ChatMessage msg);

	public class Command
	{
		public static event AsyncEventHandler<Command, CommandCalledEventArgs> CommandCalled;
		public static event AsyncEventHandler<Command, CommandExecutedEventArgs> CommandExecuted;
		public static event AsyncEventHandler<Command, CommandCancelledEventArgs> CommandCancelled;

		public event AsyncEventHandler<Command, CommandCalledEventArgs> Called;
		public event AsyncEventHandler<Command, CommandExecutedEventArgs> Executed;
		public event AsyncEventHandler<Command, CommandCancelledEventArgs> Cancelled;

		public string Name { get; private set; }
		public string Description { get; private set; } = "нет описания";
		public string Usage { get; private set; } = "<command>";
		public List<string> Aliases { get; private set; } = new List<string>();
		public List<string> Tags { get; private set; } = new List<string>();

		private readonly CommandAction _action;

		public int Rank { get; private set; } = 0;

		public bool OpOnly { get; private set; } = false;

		public bool ConsoleOnly { get; private set; } = false;

		public Command() { }
		public Command(string name, CommandAction action)
		{
			Name = name;
			_action = action;
		}

		/// <summary>
		/// Возвращает всю основную информацию о команде, в одной строке
		/// </summary>
		/// <returns></returns>
		public string GetInfo()
		{
			string info = "";
			string aliases = "";
			foreach (string alias in Aliases)
			{
				aliases += alias + (alias == Aliases[Aliases.Count-1] ? "" : ", ");
			}
			string tags = "";
			foreach (string tag in Tags)
				tags += tag + (tag == Tags[Tags.Count-1] ? "" : ", ");

			info += "Команда [" + (CommandsController.Commands.IndexOf(this) + 1) + "/" + CommandsController.Commands.Count + "]: " +
				"**" + Settings.Sign + Name + "**\n";
			info += "Аналоги: " + aliases + "\n";
			info += "Использование: " + Usage + "\n";
			info += "Описание: " + Description + "\n";
			info += "Требуемый ранг: " + Rank + "\n";
			info += "Только для админов: " + OpOnly + "\n";
			info += "Тэги: *" + tags + "*";

			info = info.Replace("<sign>", Program.Prefix + Settings.Sign).Replace("<name>", Name).Replace("<command>", Program.Prefix + Settings.Sign + Name);

			return info;
		}

		/// <summary>
		/// Задает описание команде
		/// </summary>
		/// <param name="description">Текст описания</param>
		/// <returns>Текущая команда</returns>
		public Command SetDescription(string description)
		{
			Description = description;
			return this;
		}

		/// <summary>
		/// Добавляет тег команде
		/// </summary>
		/// <param name="tag">название тега</param>
		/// <returns>Текущая команда</returns>
		public Command AddTag(string tag)
		{
			Tags.Add(tag.ToLower());
			return this;
		}

		/// <summary>
		/// Добавляет теги команде
		/// </summary>
		/// <param name="tag">название тега</param>
		/// <returns>Текущая команда</returns>
		public Command AddTags(params string[] tags)
		{
			foreach (string tag in tags)
			{
				Tags.Add(tag.ToLower());
			}
			return this;
		}

		/// <summary>
		/// Добавляет описание использования команды. Доступны теги форматирования
		/// </summary>
		/// <param name="usage">Текст описания</param>
		/// <returns>Текущая команда</returns>
		public Command SetUsage(string usage)
		{
			Usage = usage;
			return this;
		}

		/// <summary>
		/// Устанавливает минимальный ранг для использования команды
		/// </summary>
		/// <param name="rank">Минимальный ранг</param>
		/// <returns>Текущая команда</returns>
		public Command SetRank(int rank)
		{
			Rank = rank;
			return this;
		}

		/// <summary>
		/// Устанавливает необходимость иметь права оператора для этой команджы
		/// </summary>
		/// <param name="op">Только для админов?</param>
		/// <returns>Текущая команда</returns>
		public Command SetOp(bool op)
		{
			OpOnly = op;
			return this;
		}

		/// <summary>
		/// Ограничивает возможность использования только консолью
		/// </summary>
		/// <param name="console">Только для консоли?</param>
		/// <returns>Текущая команда</returns>
		public Command SetConsole(bool console)
		{
			ConsoleOnly = console;
			return this;
		}

		/// <summary>
		/// Добавляет альтернативное название команды
		/// </summary>
		/// <param name="alias">Альтернативное название</param>
		/// <returns>Текущая команда</returns>
		public Command AddAlias(string alias)
		{
			Aliases.Add(alias);
			return this;
		}


		/// <summary>
		/// Добавляет альтернативное название команды
		/// </summary>
		/// <param name="alias">Альтернативное название</param>
		/// <returns>Текущая команда</returns>
		public Command AddAliases(params string[] aliases)
		{
			foreach (var alias in aliases)
			{
				Aliases.Add(alias);
			}
			
			return this;
		}

		/// <summary>
		/// Проверяет имеет ли команда указанный тег
		/// </summary>
		/// <param name="searchingTag">Искомый тег</param>
		/// <returns></returns>
		public bool HasTag(string searchingTag)
		{
			foreach (string tag in Tags)
			{
				if (tag.ToLower() == searchingTag.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Проверяет, соответствует ли команда искомой строке (проверка по названию и псевдонимам).
		/// </summary>
		/// <param name="text">Искомая команда</param>
		/// <returns></returns>
		public bool CheckCommand(string text)
		{
			string command = text.Split(' ')[0];

			if (command.ToLower() == Name.ToLower())
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

		/// <summary>
		/// Пытается выполнить команду.
		/// </summary>
		/// <param name="message">Сообщение, содержащее команду</param>
		/// <returns>Найдена ли команда</returns>
		public async Task<bool> TryExecute(ChatMessage message)
		{
			string text = message.Text;
			Member author = message.Author;

			text = text.Substring(Program.Prefix.Length + Settings.Sign.Length, text.Length - (Program.Prefix.Length + Settings.Sign.Length));

			Logger.Debug("Попытка выполнения " + Program.Prefix + Settings.Sign + Name + " (" + text + ")");
			if (CheckCommand(text))
			{
				Logger.Debug("Обнаружена команда " + Name);
				CommandCalled?.Invoke(this, new CommandCalledEventArgs());
				Called?.Invoke(this, new CommandCalledEventArgs());
				if (author.IsOp() || author.Rank >= Rank)
				{
					if (OpOnly && !author.IsOp())
					{
						Logger.Warning("Команда только для операторов");
						return true;
					}
					CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(message));
					Executed?.Invoke(this, new CommandExecutedEventArgs(message));
					await _action(message);
				}
				CommandCancelled?.Invoke(this, new CommandCancelledEventArgs());
				Cancelled?.Invoke(this, new CommandCancelledEventArgs());
				return true;
			}
			return false;
		}
	}
}
