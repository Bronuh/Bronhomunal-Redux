using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Events
{
	/// TODO: Добавить систему событий, для улучшения читаемости кода
	class Events
	{
		public delegate void TakenDamageEventHandler(object sender, TakenDamageEventArgs eventArgs);
		public class TakenDamageEventArgs : EventArgs
		{

		}
	}
}
