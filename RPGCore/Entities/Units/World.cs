namespace RPGCore.Entities
{
	public class World : Unit
	{
		public World() : base()
		{
			AddTag("world");
		}

		/// <summary>
		/// Мир не получает урона. Но попытку засчитает. И запомнит.
		/// </summary>
		/// <param name="damage"></param>
		public override void TakeDamage(Damage damage)
		{
			//do nothing
		}
	}
}
