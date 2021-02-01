using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
	/// <summary>
	/// Хранит статистические данные пользователя. В осносном требуется для достижений
	/// </summary>
	[Serializable]
	public class MemberStatistics
	{
		public int
			StickPokes = 0,
			StickHits = 0,
			LogHits = 0,
			TreeHits = 0,
			PokedByStick = 0,
			HitByStick = 0,
			HitByLog = 0,
			HitByTree = 0,
			InfosMeasured = 0,
			WhoisSelf = 0,
			WhoisOther = 0,
			WhoisTotal = 0;


		public MemberStatistics() { }
	}
}
