using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bronuh.Logic
{

	/// <summary>
	/// Последовательность объектов. В отличие от списка List имеет методы обработки элементов массива, в т.ч. с прерыванием цикла
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public delegate bool BreakableAction<in T>(T obj);

	[Serializable]
	public class Sequence<T> : List<T>
	{
		public Sequence() : base() { }

		public Sequence(int capacity) : base(capacity) { }

		public Sequence(IEnumerable<T> collection) : base(collection) { }



		/// <summary>
		/// Проделывает действие (делегат/лямбда) для каждого свойства в списке
		/// </summary>
		/// <param name="action"></param>
		public void Each(Action<T> action)
		{
			foreach (T property in this)
			{
				action(property);
			}
		}



		/// <summary>
		/// (Для больших списков параметров) Проделывает действие над каждым свойством, возвращая bool.
		/// Возврат true - продолжать со следующим элементом в списке, false - выйти из цикла.
		/// </summary>
		/// <param name="action"></param>
		public void Each(BreakableAction<T> action)
		{
			foreach (T property in this)
			{
				if (!action(property))
				{
					break;
				}
			}
		}




		public new T Find(Predicate<T> match)
		{
			return base.Find(match);
		}




		public new Sequence<T> FindAll(Predicate<T> match)
		{
			return new Sequence<T>(base.FindAll(match));
		}
	}
}
