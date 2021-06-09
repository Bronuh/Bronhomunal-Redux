using System;
using System.Collections.Generic;
using System.Text;

namespace Bronhomunal_VK
{
	public static class Extensions
	{
		public static T GetRandom<T>(this List<T> list)
		{
			Console.WriteLine("GetRandom("+list.Count+")...");
			return list[new Random().Next(0, list.Count)];
		}
	}
}
