using Bronuh.Types;
using System.Collections.Generic;

namespace Bronuh.Controllers
{
	public class AchievementsController : IInitializable
	{
		public static readonly List<Achievement> Achievements = new List<Achievement>();

		public static Achievement StickPoke,StickHit,LogHit,TreeHit,WoodenWarrior,Riot,Major,
		CoolHacker,Btard,Ai,Wasted,Infameter,ItsNotMe,Zoologist,Zerg,Crowd,Party,Bronuh,Overwhelming,
		Alone, Ascension,Stone, Voice1,Voice2, Voice3, Voice4, Voice5, Why, Artist, Gaems, WhoAmI, About,Yes,
		No,Maybe,Chat,Veteran, Treed, Logged,Diy, Colored, Fitting, Tricky, Curious, Others;
		private Achievement NSFW;
		private Achievement TotalVoice3;
		private Achievement TotalVoice2;
		private Achievement TotalVoice1;

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
			return Achievements.Find(a => a.Id == id.ToLower());
		}

		/// <summary>
		/// Создает список достижений. Реализация выдачи достижений разбросана по всем остальным классам. 
		/// TODO: рассмотреть вариант инициализации ачивок через конструкторы, с присвоением идентификаторов 
		/// TODO: рассмотреть вопрос выноса объявления достижений в отдельный класс
		/// </summary>
		public void Initialize()
		{
			StickPoke = AddAchievement("stickpoke")
				.SetName("Оно живое?")
				.SetValue(20)
				.SetDescription("Ткните куда-нибудь веткой <value> раз")
				.SetIcon(Properties.Achievements.Stickpoke)
				.SetRarity(Rarity.COMMON);

			StickHit = AddAchievement("stickhit")
				.SetName("Обезьяна")
				.SetValue(40)
				.SetDescription("Нанесите удар палкой <value> раз")
				.SetIcon(Properties.Achievements.Stickhit)
				.SetRarity(Rarity.UNCOMMON);

			LogHit = AddAchievement("loghit")
				.SetName("Раз - и готово!")
				.SetValue(40)
				.SetDescription("Нанесите удар бревном <value> раз")
				.SetIcon(Properties.Achievements.Loghit)
				.SetRarity(Rarity.RARE);

			TreeHit = AddAchievement("treehit")
				.SetName("Чтоб наверняка")
				.SetValue(60)
				.SetDescription("Нанесите удар деревом <value> раз")
				.SetIcon(Properties.Achievements.Treehit)
				.SetRarity(Rarity.LEGENDARY);

			WoodenWarrior = AddAchievement("woodenwarrior")
				.SetName("Человек-дерево")
				.SetDescription("Заебать пользователей сервера всеми видами упоминаний")
				.SetIcon(Properties.Achievements.Woodenwarrior)
				.SetRarity(Rarity.EXOTIC);

			Riot = AddAchievement("riot")
				.SetName("Бунт")
				.SetDescription("Приложить админа к дереву")
				.SetIcon(Properties.Achievements.Riot)
				.SetRarity(Rarity.UNCOMMON);

			Major = AddAchievement("major")
				.SetName("Я все о тебе знаю")
				.SetValue(20)
				.SetDescription("Проверить инфу о пользователях <value> раз")
				.SetIcon(Properties.Achievements.Major)
				.SetRarity(Rarity.UNCOMMON);

			CoolHacker = AddAchievement("coolhacker")
				.SetName("Кулхацкир")
				.SetDescription("Написать что-нибудь в канал #dev")
				.SetIcon(Properties.Achievements.Coolhacker)
				.SetRarity(Rarity.COMMON);

			Btard = AddAchievement("btard")
				.SetName("Асоциальность")
				.SetDescription("Зачем вообще сюда писать?")
				.SetIcon(Properties.Achievements.Btard)
				.SetRarity(Rarity.COMMON);

			Ai = AddAchievement("ai")
				.SetName("Высший разум")
				.SetDescription("Зайти в канал к ботам")
				.SetIcon(Properties.Achievements.AI)
				.SetRarity(Rarity.COMMON);

			Wasted = AddAchievement("wasted")
				.SetName("ПОТРАЧЕНО")
				.SetDescription("ОХЛАДИТЬ ТРАХАНИЕ НЕКОТОРЫХ БУКВ, esse?")
				.SetIcon(Properties.Achievements.CJ)
				.SetRarity(Rarity.COMMON);

			Infameter = AddAchievement("infameter")
				.SetName("Инфамер")
				.SetDescription("Измерить какую-нибюудь инфу")
				.SetIcon(Properties.Achievements.Stonks)
				.SetRarity(Rarity.COMMON);

			ItsNotMe = AddAchievement("itsnotme")
				.SetName("Это не я")
				.SetDescription("Заставить бота что-нибудь сказать")
				.SetIcon(Properties.Achievements.Another)
				.SetRarity(Rarity.UNCOMMON);

			Zoologist = AddAchievement("zoologist")
				.SetName("Зоолог")
				.SetDescription("Застать Хотсу в её естественной среде обитания")
				.SetIcon(Properties.Achievements.Zoologist)
				.SetRarity(Rarity.UNCOMMON);

			Zerg = AddAchievement("zerg")
				.SetName("Зерги")
				.SetValue(10)
				.SetDescription("Побывать в войсе толпой в <value> человек и больше")
				.SetIcon(Properties.Achievements.Zerg)
				.SetRarity(Rarity.EXOTIC);

			Crowd = AddAchievement("crowd")
				.SetName("Толпа")
				.SetValue(8)
				.SetDescription("Побывать в войсе толпой в <value> человек и больше")
				.SetIcon(Properties.Achievements.Crowd)
				.SetRarity(Rarity.RARE);

			Party = AddAchievement("party")
				.SetName("Пати")
				.SetValue(6)
				.SetDescription("Побывать в войсе толпой в <value> человек и больше")
				.SetIcon(Properties.Achievements.Party)
				.SetRarity(Rarity.UNCOMMON);

			Bronuh = AddAchievement("bronuh")
				.SetName("Лови его!")
				.SetDescription("Побывать с Бронухом в одном войс чате")
				.SetIcon(Properties.Achievements.Bronuh)
				.SetRarity(Rarity.RARE);

			Overwhelming = AddAchievement("overwhelming")
				.SetName("Мощь переполняет")
				.SetDescription("Попробовать все способы доебаться до кого-то")
				.SetIcon(Properties.Achievements.Overwhelming)
				.SetRarity(Rarity.UNCOMMON);

			Alone = AddAchievement("alone")
				.SetName("А где все?")
				.SetDescription("Зайти в пустой голосовой канал")
				.SetIcon(Properties.Achievements.Alone)
				.SetRarity(Rarity.COMMON);

			Ascension = AddAchievement("ascension")
				.SetName("Теперь я видел все")
				.SetDescription("Застать в войсе говорящего Бронуха")
				.SetIcon(Properties.Achievements.Ascension)
				.SetRarity(Rarity.LEGENDARY);

			Stone = AddAchievement("stone")
				.SetName("Я — камень")
				.SetDescription("Зайти в войс и выключить звук")
				.SetIcon(Properties.Achievements.Stone)
				.SetRarity(Rarity.COMMON);

			Voice1 = AddAchievement("voice1")
				.SetName("Алло")
				.SetValue(1000*60)
				.SetDescription("Просидеть в войсе минуту, не выходя")
				.SetIcon(Properties.Achievements.Voice_1)
				.SetRarity(Rarity.COMMON);

			Voice2 = AddAchievement("voice2")
				.SetName("АЛЛО Я СКАЗАЛ!")
				.SetValue(1000*60*10)
				.SetDescription("Просидеть в войсе 10 минут, не выходя")
				.SetIcon(Properties.Achievements.Voice_2)
				.SetRarity(Rarity.UNCOMMON);

			Voice3 = AddAchievement("voice3")
				.SetName("Попиздим?")
				.SetValue(1000 * 60 * 60)
				.SetDescription("Не вылезать из войса час")
				.SetIcon(Properties.Achievements.Voice_3)
				.SetRarity(Rarity.RARE);

			Voice4 = AddAchievement("voice4")
				.SetName("Общительный")
				.SetValue(1000 * 60 * 60 * 6)
				.SetDescription("Просидеть в войсе 6 часов подряд")
				.SetIcon(Properties.Achievements.Voice_4)
				.SetRarity(Rarity.LEGENDARY);

			Voice5 = AddAchievement("voice5")
				.SetName("Неумолкаемый")
				.SetValue(1000 * 60 * 60 * 12)
				.SetDescription("Просидеть в войсе 12 часов подряд")
				.SetIcon(Properties.Achievements.Voice_5)
				.SetRarity(Rarity.EXOTIC);

			Why = AddAchievement("why")
				.SetName("1001100 1001111 1001100")
				.SetDescription("Доебаться до бота")
				.SetIcon(Properties.Achievements.Why)
				.SetRarity(Rarity.UNCOMMON);
			// ==============================================================================================================

			Artist = AddAchievement("artist")
				.SetName("Художнек")
				.SetDescription("Может быть обижен каждым")
				.SetIcon(Properties.Achievements.Artist)
				.SetRarity(Rarity.COMMON);

			Gaems = AddAchievement("gaems")
				.SetName("Игорь тонет")
				.SetDescription("Завалиться в геймерскую тусовку")
				.SetIcon(Properties.Achievements.Gaems)
				.SetRarity(Rarity.COMMON);

			WhoAmI = AddAchievement("whoami")
				.SetName("Я кто?")
				.SetValue(20)
				.SetDescription("Проверить инфу о себе <value> раз")
				.SetIcon(Properties.Achievements.WhoAmI)
				.SetRarity(Rarity.COMMON);

			About = AddAchievement("about")
				.SetName("Прописка")
				.SetDescription("Написать инфу о себе")
				.SetIcon(Properties.Achievements.About)
				.SetRarity(Rarity.COMMON);

			Yes = AddAchievement("yes")
				.SetName("Концентрат")
				.SetDescription("Получить сверхчистую 100% инфу")
				.SetIcon(Properties.Achievements.Yes)
				.SetRarity(Rarity.EXOTIC);

			No = AddAchievement("no")
				.SetName("Ниет")
				.SetDescription("Получить нечто с содержанием инфы 0%")
				.SetIcon(Properties.Achievements.No)
				.SetRarity(Rarity.RARE);

			Maybe = AddAchievement("maybe")
				.SetName("Хз")
				.SetDescription("Даже Великий Рандом не знает ответа на этот вопрос")
				.SetIcon(Properties.Achievements.Maybe)
				.SetRarity(Rarity.LEGENDARY);

			Chat = AddAchievement("chat")
				.SetName("Буквометчик")
				.SetValue(250)
				.SetDescription("Так много букав, что можно было бы написать книгу")
				.SetIcon(Properties.Achievements.Chat)
				.SetRarity(Rarity.RARE);

			Veteran = AddAchievement("veteran")
				.SetName("Ветеран")
				.SetValue(10)
				.SetDescription("Достичь ранга <value>")
				.SetIcon(Properties.Achievements.Veteran)
				.SetRarity(Rarity.LEGENDARY);

			Treed = AddAchievement("treed")
				.SetName("Дуболом")
				.SetValue(30)
				.SetDescription("Получить <value> ударов деревом")
				.SetIcon(Properties.Achievements.Treed)
				.SetRarity(Rarity.LEGENDARY);

			Logged = AddAchievement("logged")
				.SetName("Пень")
				.SetValue(30)
				.SetDescription("Получить <value> ударов бревном")
				.SetIcon(Properties.Achievements.Logged)
				.SetRarity(Rarity.RARE);

			Diy = AddAchievement("diy")
				.SetName("Сделай сам")
				.SetDescription("Создать новую роль")
				.SetIcon(Properties.Achievements.DIY)
				.SetRarity(Rarity.UNCOMMON);

			Colored = AddAchievement("colored")
				.SetName("Цвет настроения...")
				.SetDescription("Разукрасить доступные роли как по кайфу")
				.SetIcon(Properties.Achievements.Colored)
				.SetRarity(Rarity.UNCOMMON);

			Fitting = AddAchievement("fitting")
				.SetName("Примерка")
				.SetDescription("Выдать себе существующую роль")
				.SetIcon(Properties.Achievements.Fitting)
				.SetRarity(Rarity.UNCOMMON);

			Tricky = AddAchievement("tricky")
				.SetName("Хитрая жопа")
				.SetDescription("Попытаться выполнить команду от лица бота")
				.SetIcon(Properties.Achievements.Tricky)
				.SetRarity(Rarity.UNCOMMON);

			Curious = AddAchievement("curious")
				.SetName("Любопытный")
				.SetDescription("Попытаться получить список админских команд")
				.SetIcon(Properties.Achievements.Curious)
				.SetRarity(Rarity.RARE);

			Others = AddAchievement("others")
				.SetName("Чё тут у нас")
				.SetDescription("Посмотреть чужие ачивки")
				.SetIcon(Properties.Achievements.Others)
				.SetRarity(Rarity.COMMON);

			NSFW = AddAchievement("nsfw")
				.SetName("NSFW")
				.SetDescription("\"Случайно\" заглянуть в NSFW канал")
				.SetIcon(Properties.Achievements.NSFW)
				.SetRarity(Rarity.COMMON);

			TotalVoice3 = AddAchievement("totalvoice3")
				.SetName("Войс 3")
				.SetValue(12)
				.SetDescription("Просидеть в войсе суммарно <value> часов")
				.SetIcon(Properties.Achievements.TotalVoice3)
				.SetRarity(Rarity.UNCOMMON);

			TotalVoice2 = AddAchievement("totalvoice2")
				.SetName("Войс 2")
				.SetValue(24)
				.SetDescription("Просидеть в войсе суммарно <value> часов")
				.SetIcon(Properties.Achievements.TotalVoice2)
				.SetRarity(Rarity.RARE);

			TotalVoice1 = AddAchievement("totalvoice1")
				.SetName("Войс 1")
				.SetValue(48)
				.SetDescription("Просидеть в войсе суммарно <value> часов")
				.SetIcon(Properties.Achievements.TotalVoice1)
				.SetRarity(Rarity.LEGENDARY);
		}
	}
}
