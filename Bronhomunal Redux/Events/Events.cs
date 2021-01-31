using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Events
{
	class Events
	{
		public delegate void TakenDamageEventHandler(object sender, TakenDamageEventArgs eventArgs);
		public class TakenDamageEventArgs : EventArgs
		{

		}
	}
}
