using System;
using System.Collections.Generic;
using System.Text;

namespace Bronuh.Types
{
	public class Alias
	{
		public string Name;
		public ulong ID;

		public Alias()
		{
			Name = "";
			ID = 0;
		}

		public Alias(string name, ulong id)
		{
			Name = name;
			ID = id;
		}

	}
}
