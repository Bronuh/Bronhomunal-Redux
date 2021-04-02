using Bronuh.Libs;
using Bronuh.Types;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

namespace Bronuh.Controllers.Commands
{
	class Memes : ICommands
	{
		private Dictionary<string, int> memeIds = new Dictionary<string, int>();

		public void InitializeCommands()
		{
			CommandsController.AddCommand("killfrog", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				await m.RespondAsync(new DiscordMessageBuilder()
					.WithFile("Killfrog.gif", Properties.Memes.Killfrog.ToGifStream()));
			})
			.AddAlias("киллфрог")
			.SetDescription("Постит киллфрога")
			.AddTag("meme");


			CommandsController.AddCommand("hmm", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("ItsOkay.png", GetMemeStream("Hmm"));

				await m.RespondAsync(builder);
			})
			.AddAliases("хмм", "думает", "думоет")
			.SetDescription("Постит мыслительный процесс")
			.AddTag("meme");


			CommandsController.AddCommand("сойдет", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();
				
				builder.WithFile("ItsOkay.png", GetMemeStream("GoodEnough"));


				await m.RespondAsync(builder);
			})
			.AddAliases("итаксойдет", "похуй", "норм")
			.SetDescription("Вам, вероятно, похуй")
			.AddTag("meme");

			CommandsController.AddCommand("dosomething", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Work.png", GetMemeStream("Work"));


				await m.RespondAsync(builder);
			})
			.AddAliases("Работай")
			.SetDescription("Женя, блятб")
			.AddTag("meme");

			CommandsController.AddCommand("dumb", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Dumb.png", GetMemeStream("Dumb"));

				await m.RespondAsync(builder);
			})
			.AddAliases("тупой", "ыыы", "гы")
			.SetDescription("Постит мыслительный процесс")
			.AddTag("meme");

			CommandsController.AddCommand("omg", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Omg.png", GetMemeStream("Omg"));

				await m.RespondAsync(builder);
			})
			.AddAliases("ничоси", "ничосе", "хуясе", "хуяси")
			.SetDescription("Крайняя степень нихуясебевания")
			.AddTag("meme");

			CommandsController.AddCommand("uuu", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Uuu.png", GetMemeStream("Uuu"));

				await m.RespondAsync(builder);
			})
			.AddAliases("ууу", "ъуъ", "уъу")
			.SetDescription("УЪУ!")
			.AddTag("meme");

			CommandsController.AddCommand("plan", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Plan.png", GetMemeStream("Plan"));

				await m.RespondAsync(builder);
			})
			.AddAliases("план", "надежно", "охуенныйплан")
			.SetDescription("Великолепный план, просто охуенный если я правильно понял. Надежный, блять, как швейцарские часы.")
			.AddTag("meme");

			CommandsController.AddCommand("fuck", async (m) =>
			{
				string text = m.Text;
				string[] parts = text.Split(' ');
				int userRank = m.Author.Rank;
				Member sender = m.Author;

				var builder = new DiscordMessageBuilder();

				builder.WithFile("Fuck.png", GetMemeStream("Fuck"));

				await m.RespondAsync(builder);
			})
			.AddAliases("пиздец", "oof", "больно")
			.SetDescription("Что-то пошло не по плану...")
			.AddTag("meme");
		}




		private Stream GetMemeStream(string memeName)
		{
			var props = new List<PropertyInfo>(typeof(Properties.Memes).GetProperties().Where(p => p.Name.StartsWith(memeName)));
			if (!memeIds.ContainsKey(memeName))
				memeIds.Add(memeName,1);
			int count = props.Count();

			//var bmp = (System.Drawing.Bitmap)props[memeIds[memeName]-1].GetValue(null);
			var bmp = (System.Drawing.Bitmap)props.GetRandom().GetValue(null);

			memeIds[memeName]++;
			memeIds[memeName] = memeIds[memeName] == (count + 1) ? 1 : memeIds[memeName];

			return bmp.ToStream();
		}
	}
}
