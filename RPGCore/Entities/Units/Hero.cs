using RPGCore.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore.Entities
{
    // TODO: Переделать систему опыта
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
        public int XP, SkillPoints = 1;
        public static int BaseLevelXP = 10;
        public static double PerLevelXPMult = 1.5;
        public static double PerLevelStep = 5;


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
            return LevelXP(Level + 1);
        }




        /// <summary>
        /// Возвращает требуемое количество опыта для получения указанного уровня
        /// </summary>
        /// <param name="level">Требуемый уровень</param>
        /// <returns></returns>
        public int LevelXP(int level)
        {
            level = Math.Max(level, 1);
            int currentXP = BaseLevelXP;

            if (level == 1)
            {
                return currentXP;
            }

            for (int i = 1; i <= level; i++)
            {
                currentXP += (int)(PerLevelStep * (i));
            }

            return currentXP;

        }




        /// <summary>
        /// Возвращает значение уровня, достигаемое при заданном количестве опыта
        /// </summary>
        /// <param name="xp">Требуемый опыт</param>
        /// <returns></returns>
        public int XPLevel(int xp)
        {
            bool got = false;
            int level = 1;

            while (!got || level < 200)
            {
                if (LevelXP(level) > xp)
                {
                    return level - 1;
                }

            }
            return 200;
        }

        public virtual void LevelUp()
        {
            Level++;
            SkillPoints++;
        }


    }
}
