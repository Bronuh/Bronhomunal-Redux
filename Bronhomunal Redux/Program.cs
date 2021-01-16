using System;
using System.Threading;
using System.Threading.Tasks;
using Bronuh.Events;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bronuh
{
	class Program
	{
		static void Main(string[] args)
		{
			Logger.Log("Загрузка...");
			MembersController.Load();

			Logger.Log("Инициализация бота...");

			new Thread(new ThreadStart(() =>
			{
				Bot.Initialize("Njk2OTUyMTgzNTcyMjY3MDI4.XowNTQ.TYfUocBBYS4BHZv4gWbbMy1fGNI");
			})).Start();


			CommandsController.AddCommand("shutdown", async e =>
			{
				DiscordUser sender = e.Author.Source;
				if (sender.IsOwner())
				{
					SaveAll();
					System.Diagnostics.Process.Start("cmd", "/c shutdown /s /t 0");
				}
			});

			CommandsController.AddCommand("kill", async e =>
			{
				DiscordUser sender = e.Author.Source;
				if (sender.IsOwner())
				{
					SaveAll();
					Environment.Exit(0);
				}
			});
		}

		public static void SaveAll()
		{
			MembersController.Save();
			AliasesController.Save();
		}

		public static void Initialize()
		{

		}
	}
}
