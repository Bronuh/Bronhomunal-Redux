using System;
using System.Collections.Generic;
using System.Text;
using Bronuh.Types;

namespace Bronuh.Controllers
{
	public class AchievementsController : IInitializable
	{
		public static readonly List<Achievement> Achievements = new List<Achievement>();


		public static Achievement AddAchievement(Achievement achievement)
		{
			Achievements.Add(achievement);
			return achievement;
		}

		public static Achievement AddAchievement(string id)
		{
			Achievement achievement = new Achievement(id);
			Achievements.Add(achievement);
			return achievement;
		}

		public static Achievement Find(string id)
		{
			return Achievements.Find(a=>a.Id==id.ToLower());
		}

		public void Initialize()
		{
			AddAchievement("stickpoke10times")
				.SetName("Оно живое?")
				.SetDescription("Ткните веткой 10 раз")
				.SetIcon(Properties.Achievements.Stickpoke)
				.SetRarity(Rarity.COMMON);

			AddAchievement("stickhit20times")
				.SetName("Надоедливый")
				.SetDescription("Ударьте палкой 20 раз")
				.SetIcon(Properties.Achievements.Stickhit)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("loghit20times")
				.SetName("Раз - и готово!")
				.SetDescription("Ударьте бревном 20 раз")
				.SetIcon(Properties.Achievements.Loghit)
				.SetRarity(Rarity.RARE);
			
			AddAchievement("treehit30times")
				.SetName("Чтоб наверняка")
				.SetDescription("Ударьте деревом 30 раз")
				.SetIcon(Properties.Achievements.Treehit)
				.SetRarity(Rarity.LEGENDARY);

			AddAchievement("woodenwarrior")
				.SetName("Человек-дерево")
				.SetDescription("Заебать пользователей сервера всеми видами упоминаний")
				.SetIcon(Properties.Achievements.Woodenwarrior)
				.SetRarity(Rarity.EXOTIC);
		}
	}
}
