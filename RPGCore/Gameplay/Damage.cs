using RPGCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCore
{
    [Serializable]
    public class Damage
    {
        public Unit Source;

        public double BaseDamage;
        public List<TagDamage> Tags = new List<TagDamage>();

        public Damage() { }
        public Damage(double Amount) { BaseDamage = Amount; }




        /// <summary>
        /// Создает копию урона
        /// </summary>
        /// <returns></returns>
        public Damage Copy()
        {
            Damage damage = new Damage(BaseDamage);
            foreach (TagDamage tag in Tags)
            {
                damage.Tags.Add(new TagDamage() { Tag = tag.Tag, Value = tag.Value });
            }
            damage.Source = Source;
            return damage;
        }





        /// <summary>
        /// Добавляет к текущему урону показатели другого урона
        /// </summary>
        /// <param name="other"></param>
        public void Add(Damage other)
        {
            foreach (TagDamage tag in other.Tags)
            {
                if (HasTag(tag.Tag))
                {
                    GetTag(tag.Tag).Value += tag.Value;
                }
                else
                {
                    Tags.Add(tag);
                }
            }
        }




        /// <summary>
        /// Проверяет наличие тега в уроне
        /// </summary>
        /// <param name="tagName">Название искомого тега</param>
        /// <returns></returns>
        public bool HasTag(string tagName)
        {
            foreach (TagDamage tag in Tags)
            {
                if (tag.Tag.ToLower().Equals(tagName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }




        /// <summary>
        /// Возвращает ссылку на искомый тег
        /// </summary>
        /// <param name="tagName">Название искомого тега</param>
        /// <returns></returns>
        public TagDamage GetTag(string tagName)
        {
            foreach (TagDamage tag in Tags)
            {
                if (tag.Tag.ToLower().Equals(tagName.ToLower()))
                {
                    return tag;
                }
            }
            return null;
        }

    }

    /// <summary>
    /// Пара имя-значение
    /// </summary>
    public class TagDamage
    {
        public TagDamage() { }
        public TagDamage(string tag, double value) {
            Tag = tag;
            Value = value;
        }
        public string Tag;
        public double Value;
    }

}
