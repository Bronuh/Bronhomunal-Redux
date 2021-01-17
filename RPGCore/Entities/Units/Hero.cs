using RPGCore.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Entities
{
    [Serializable]
    public class Hero : Unit
    {
        /// <summary>
        /// Имя персонажа
        /// </summary>
        public string CharacterName;
        /// <summary>
        /// Мешок. Эффект от предметов, находящихся в нем не учитывается.
        /// </summary>
        public Inventory Bag = new Inventory() { Limited = false };
        public int XP = 0;
        public int SkillPoints = 1;
        public static int XpPerLevel = 1000;



        public Hero()
        {
            Inventory = new Inventory() { Limited = true, Size = 6};
        }



        /// <summary>
        /// Расчитывает требуемое количество опыта для перехода на следующий уровень.
        /// </summary>
        /// <returns>Возвращает количество опыта, требуемого для достижения следующего уровня</returns>
        public int ToNextLevel()
        {
            return LevelXP(Level + 1)-XP;
        }



        public void AddXP(int xp)
		{
            XP += xp;
			if (LevelForXp(XP)>Level)
			{
                int levels = LevelForXp(XP) - Level;
				for (int i = 1; i<=levels; )
				{
                    LevelUp();
				}
			}
		}


        /// <summary>
        /// Возвращает требуемое количество опыта для получения указанного уровня
        /// </summary>
        /// <param name="level">Требуемый уровень</param>
        /// <returns></returns>
        public int LevelXP(int level)
        {
            return XpPerLevel * (level - 1);

        }




        /// <summary>
        /// Возвращает значение уровня, достигаемое при заданном количестве опыта
        /// </summary>
        /// <param name="xp">Требуемый опыт</param>
        /// <returns></returns>
        public int LevelForXp(int xp)
        {
            return (int)Math.Floor((double)xp / XpPerLevel)+1;
        }

        public virtual void LevelUp()
        {
            Level++;
            SkillPoints++;
        }


    }
}
