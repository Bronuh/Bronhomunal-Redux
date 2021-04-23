using System;
using System.Collections.Generic;
using System.Text;

namespace Bronhomunal_VK
{
	public static class Memes
	{
		public static List<Meme> memes { get; private set; } = new List<Meme>();


		/// <summary>
		/// Добавляет команду в общий список
		/// </summary>
		/// <param name="name">Сама команда</param>
		/// <param name="action">Делегат действия</param>
		/// <returns>Ссылка на добавленную команду</returns>
		public static Meme AddMeme(Meme meme)
		{
			memes.Add(meme);
			return meme;
		}

		public static Meme FindMeme(string name)
		{
			Console.WriteLine("Trying to fend memi...");
			Meme command = null;

			foreach (var cmd in memes)
			{
				if (cmd.CheckMeme(name))
				{
					command = cmd;
					break;
				}
			}

			return command;
		}
	}
}
