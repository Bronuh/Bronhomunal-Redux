using System;
using System.Collections.Generic;
using System.Text;
using Bronuh.Types;

namespace Bronuh.Controllers
{
	public class AchievementsController : IInitializable
	{
		public static readonly List<Achievement> Achievements = new List<Achievement>();

		/// <summary>
		/// Добавляет достижение в публичный статический список.
		/// </summary>
		/// <param name="achievement"></param>
		/// <returns>Добавленное достижение для дальнейшей конфигурации</returns>
		public static Achievement AddAchievement(Achievement achievement)
		{
			Achievements.Add(achievement);
			return achievement;
		}

		/// <summary>
		/// Создает и добавляет достижение в публичный статический список.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Добавленное достижение для дальнейшей конфигурации</returns>
		public static Achievement AddAchievement(string id)
		{
			Achievement achievement = new Achievement(id);
			Achievements.Add(achievement);
			return achievement;
		}


		/// <summary>
		/// Находит достижение по его Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Найденное достижение, либо null</returns>
		public static Achievement Find(string id)
		{
			return Achievements.Find(a=>a.Id==id.ToLower());
		}


		/// <summary>
		/// Создает список достижений. Реализация выдачи достижений разбросана по всем остальным классам. 
		/// TODO: рассмотреть вариант инициализации ачивок через конструкторы, с присвоением идентификаторов 
		/// TODO: рассмотреть вопрос выноса объявления достижений в отдельный класс
		/// </summary>
		public void Initialize()
		{
			AddAchievement("stickpoke")
				.SetName("Оно живое?")
				.SetDescription("Ткните куда-нибудь веткой 10 раз")
				.SetIcon(Properties.Achievements.Stickpoke)
				.SetRarity(Rarity.COMMON);

			AddAchievement("stickhit")
				.SetName("Обезьяна")
				.SetDescription("Нанесите удар палкой 20 раз")
				.SetIcon(Properties.Achievements.Stickhit)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("loghit")
				.SetName("Раз - и готово!")
				.SetDescription("Нанесите удар бревном 20 раз")
				.SetIcon(Properties.Achievements.Loghit)
				.SetRarity(Rarity.RARE);
			
			AddAchievement("treehit")
				.SetName("Чтоб наверняка")
				.SetDescription("Нанесите удар деревом 30 раз")
				.SetIcon(Properties.Achievements.Treehit)
				.SetRarity(Rarity.LEGENDARY);

			AddAchievement("woodenwarrior")
				.SetName("Человек-дерево")
				.SetDescription("Заебать пользователей сервера всеми видами упоминаний")
				.SetIcon(Properties.Achievements.Woodenwarrior)
				.SetRarity(Rarity.EXOTIC);

			AddAchievement("riot")
				.SetName("Бунт")
				.SetDescription("Приложить админа к дереву")
				.SetIcon(Properties.Achievements.Riot)
				.SetRarity(Rarity.RARE);

			AddAchievement("major")
				.SetName("Я все о тебе знаю")
				.SetDescription("Проверить инфу о пользователях 10 раз")
				.SetIcon(Properties.Achievements.Major)
				.SetRarity(Rarity.RARE);

			AddAchievement("coolhacker")
				.SetName("Кулхацкир")
				.SetDescription("Написать что-нибудь в канал #dev")
				.SetIcon(Properties.Achievements.Coolhacker)
				.SetRarity(Rarity.COMMON);

			AddAchievement("btard")
				.SetName("Асоциальность")
				.SetDescription("Зачем вообще сюда писать?")
				.SetIcon(Properties.Achievements.Btard)
				.SetRarity(Rarity.COMMON);

			AddAchievement("ai")
				.SetName("Высший разум")
				.SetDescription("Зайти в канал к ботам")
				.SetIcon(Properties.Achievements.AI)
				.SetRarity(Rarity.COMMON);

			AddAchievement("wasted")
				.SetName("ПОТРАЧЕНО")
				.SetDescription("ОХЛАДИТЬ ТРАХАНИЕ НЕКОТОРЫХ БУКВ, esse?")
				.SetIcon(Properties.Achievements.CJ)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("infameter")
				.SetName("Инфамер")
				.SetDescription("Измерить какую-нибюудь инфу")
				.SetIcon(Properties.Achievements.Stonks)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("itsnotme")
				.SetName("Это не я")
				.SetDescription("Заставить бота что-нибудь сказать")
				.SetIcon(Properties.Achievements.Another)
				.SetRarity(Rarity.RARE);

			AddAchievement("zoologist")
				.SetName("Зоолог")
				.SetDescription("Застать Хотсу в её естественной среде обитания")
				.SetIcon(Properties.Achievements.Zoologist)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("zerg")
				.SetName("Зерги")
				.SetDescription("Побывать в войсе толпой в 10 человек и больше")
				.SetIcon(Properties.Achievements.Zerg)
				.SetRarity(Rarity.EXOTIC);

			AddAchievement("crowd")
				.SetName("Толпа")
				.SetDescription("Побывать в войсе толпой в 8 человек и больше")
				.SetIcon(Properties.Achievements.Crowd)
				.SetRarity(Rarity.RARE);

			AddAchievement("party")
				.SetName("Пати")
				.SetDescription("Побывать в войсе толпой в 6 человек и больше")
				.SetIcon(Properties.Achievements.Party)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("bronuh")
				.SetName("Лови его!")
				.SetDescription("Побывать с Бронухом в одном войс чате")
				.SetIcon(Properties.Achievements.Bronuh)
				.SetRarity(Rarity.LEGENDARY);

			AddAchievement("overwhelming")
				.SetName("Мощь переполняет")
				.SetDescription("Попробовать все способы доебаться до кого-то")
				.SetIcon(Properties.Achievements.Overwhelming)
				.SetRarity(Rarity.RARE);

			AddAchievement("alone")
				.SetName("А где все?")
				.SetDescription("Зайти в пустой голосовой канал")
				.SetIcon(Properties.Achievements.Alone)
				.SetRarity(Rarity.COMMON);

			AddAchievement("ascension")
				.SetName("Теперь я видел все")
				.SetDescription("Застать в войсе говорящего Бронуха")
				.SetIcon(Properties.Achievements.Ascension)
				.SetRarity(Rarity.EXOTIC);

			AddAchievement("stone")
				.SetName("Я — камень")
				.SetDescription("Зайти в войс и выключить звук")
				.SetIcon(Properties.Achievements.Stone)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("voice1")
				.SetName("Алло")
				.SetDescription("Просидеть в войсе минуту")
				.SetIcon(Properties.Achievements.Voice_1)
				.SetRarity(Rarity.COMMON);

			AddAchievement("voice2")
				.SetName("АЛЛО Я СКАЗАЛ!")
				.SetDescription("Просидеть в войсе 10 минут")
				.SetIcon(Properties.Achievements.Voice_2)
				.SetRarity(Rarity.UNCOMMON);

			AddAchievement("voice3")
				.SetName("Попиздим?")
				.SetDescription("Просидеть в войсе час")
				.SetIcon(Properties.Achievements.Voice_3)
				.SetRarity(Rarity.RARE);

			AddAchievement("voice4")
				.SetName("Общительный")
				.SetDescription("Просидеть в войсе 6 часов")
				.SetIcon(Properties.Achievements.Voice_4)
				.SetRarity(Rarity.LEGENDARY);

			AddAchievement("voice5")
				.SetName("Неумолкаемый")
				.SetDescription("Просидеть в войсе 12 часов")
				.SetIcon(Properties.Achievements.Voice_5)
				.SetRarity(Rarity.EXOTIC);

			AddAchievement("why")
				.SetName("1001100 1001111 1001100")
				.SetDescription("Доебаться до бота")
				.SetIcon(Properties.Achievements.Why)
				.SetRarity(Rarity.RARE);


		}
	}
}
