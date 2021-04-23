using Bronuh;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bronhomunal_VK
{
	public class Meme
	{
		public string Name { get; private set; }
		public List<string> Aliases { get; private set; } = new List<string>();

		public static List<String> files = new List<string>();

		public Meme(string name)
		{
			Name = name;
		}
		public Meme() { }

		/// <summary>
		/// Добавляет альтернативное название команды
		/// </summary>
		/// <param name="alias">Альтернативное название</param>
		/// <returns>Текущая команда</returns>
		public Meme AddAlias(string alias)
		{
			Aliases.Add(alias);
			return this;
		}


		/// <summary>
		/// Добавляет альтернативное название команды
		/// </summary>
		/// <param name="alias">Альтернативное название</param>
		/// <returns>Текущая команда</returns>
		public Meme AddAliases(params string[] aliases)
		{
			foreach (var alias in aliases)
			{
				Aliases.Add(alias);
			}

			return this;
		}

		/// <summary>
		/// Проверяет, соответствует ли команда искомой строке (проверка по названию и псевдонимам).
		/// </summary>
		/// <param name="text">Искомая команда</param>
		/// <returns></returns>
		public bool CheckMeme(string text)
		{
			string command = text.Replace("!", "").Split(' ')[0];

			if (command.ToLower() == Name.ToLower())
			{
				return true;
			}
			else
			{
				foreach (string alias in Aliases)
				{
					if (command.ToLower() == alias.ToLower())
					{
						return true;
					}
				}
			}
			return false;
		}

		internal static void Initialize()
		{
			DirectoryInfo dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory,"Assets","Memes"));
			Array.ForEach(dir.GetFiles(),(f) => { files.Add(f.Name); });
		}

		public string GetMemeFile()
		{
			return GetMemeFile(Name);
		}

		static string GetMemeFile(string name)
		{
			Console.WriteLine("Getting random file...");
			var mems = new List<String>(files.Where(f => f.ToLower().StartsWith(name.ToLower())));

			return Path.Combine(Environment.CurrentDirectory,"AssEts","Memes",mems.GetRandom());
		}


	}
}
