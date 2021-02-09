using System;

namespace Bronuh.Types
{
	/// <summary>
	/// Хранит статистические данные пользователя. В осносном требуется для достижений
	/// </summary>
	[Serializable]
	public class MemberStatistics
	{
		[About("Тыкнул палкой (раз)")]
		public int StickPokes = 0;
		[About("Ударил палкой (раз)")]
		public int StickHits = 0;
		[About("Ударил бревном (раз)")]
		public int LogHits = 0;
		[About("Ударил деревом (раз)")]
		public int TreeHits = 0;
		[About("Тыкнут палкой (раз)")]
		public int PokedByStick = 0;
		[About("Ударен палкой (раз)")]
		public int HitByStick = 0;
		[About("Ударен бревном (раз)")]
		public int HitByLog = 0;
		[About("Ударен деревом (раз)")]
		public int HitByTree = 0;
		[About("Измерил инфу (раз)")]
		public int InfosMeasured = 0;
		[About("Посмотрел инфу о себе (раз)")]
		public int WhoisSelf = 0;
		[About("Посмотрел инфу о других (раз)")]
		public int WhoisOther = 0;
		[About("Посмотрел инфу (раз)")]
		public int WhoisTotal = 0;
		[About("Всего времени проведено в войсе")]
		public Time VoiceTime = new Time{ value = 0 };
		[About("Самая продолжительная сессия в войсе")]
		public Time MaxVoiceSessionTime = new Time { value = 0 };
		[About("Отправлено сообщений")]
		public int SentMessages = 0;

		public MemberStatistics() { }

		[Serializable]
		public struct Time
		{
			public long value;

			public Time(long val) { value = val; }
			public Time(Time other) { value = other.value; }

			public override string ToString()
			{
				TimeSpan span = TimeSpan.FromMilliseconds(value);
				return String.Format("{0}:{1:d2}:{2:d2}",span.Hours,span.Minutes,span.Seconds);
			}

			public static Time operator +(Time time, long num)
			{
				time.value += num;
				return time;
			}

			public static Time operator +(long num, Time time)
			{
				time.value += num;
				return time;
			}

			public static Time operator +(Time time1, Time time2)
			{
				return new Time(time1.value+time2.value);
			}

			
		}
	}
}
