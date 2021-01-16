using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bronuh.Libs
{
    public class Wastificator
    {
        private static Random Rnd = new Random();
        private static bool Initialized = false;


        public static string Wastificate(String Arg)
        {
            if (!Initialized)
            {
                Initialize();
            }

            Rnd = new Random(1);
            Arg = Arg.ToUpper();

            string[] Words = Arg.Split(' ');
            String _Arg = "";
            foreach (String word in Words)
            {

                _Arg += RandomWord.TryPaste(Word.Parse(word)) + " ";

            }
            Arg = _Arg;
            _Arg = "";


            foreach (char Char in Arg)
            {
                char _Char = Char;
                if (_Char == 'С')
                {
                    if (prob(0.05))
                    {
                        _Char = 'Ы';
                    }
                }

                if (_Char == 'Д')
                {
                    if (prob(0.005))
                    {
                        _Char = 'В';
                    }
                }

                if (_Char == 'S')
                {
                    if (prob(0.9))
                    {
                        _Char = 'Ы';
                    }
                }

                if (_Char == 'Р')
                {
                    if (prob(0.1))
                    {
                        _Char = 'Я';
                    }
                }

                if (_Char == 'Г')
                {
                    if (prob(0.1))
                    {
                        _Char = 'Ж';
                    }
                }

                if (_Char == 'Е')
                {
                    if (prob(0.03))
                    {
                        _Char = 'З';
                    }
                }

                if (_Char == 'П')
                {
                    if (prob(0.01))
                    {
                        _Arg += "ТТ";
                        continue;
                    }
                }

                if (_Char == '.')
                {
                    if (prob(0.01))
                    {
                        _Arg += ", esse?";
                        continue;
                    }
                }

                if (_Char == '.')
                {
                    if (prob(0.01))
                    {
                        _Arg += "(ФЫРКАЮТ).";
                        continue;
                    }
                }

                _Arg += _Char;
            }

            Arg = _Arg;
            Arg = Arg.Replace("Ъ", "Ь").Replace("Э", "З").Replace("Щ", "Ш").Replace("4", "Ч").Replace("6", "Б").Replace("Й", "И");

            return Arg;
        }

        internal static bool prob(double Prob)
        {
            return (Rnd.NextDouble() <= Prob);
        }

        private static void Initialize()
        {
            new Word("БЛЕНДЕР", "БЛЗНДЗР", 1);
            new Word("ЖЕНЩИНА", "САРДУЛЬКА", 0.5);
            new Word("ТАК", "АТК", 0.7);
            new Word("ЧЕЛОВЕК", "ЛЕЖОФЕК", 0.5);
            new Word("НАРКОТИК", "yay", 0.5);
            new Word("НАРКОТИК", "ТРЕШИНА", 0.5);
            new Word("ТЫ", "Ю", 0.3);
            new Word("ШКАЛА", "БРУСОК", 0.5);
            new Word("БЕЖЕВ", "ЬБЕЖЕВ", 1);
            new Word("ВЕЩИ", "ЦВЕТА", 0.1);
            new Word("ВЕЩИ", "НИТИ", 0.2);
            new Word("МНОГО", "СНОГО", 0.2);
            new Word("ПИДОР", "БОЛЛА", 0.5);
            new Word("ПРОСТО", "ПРОСИТО", 0.3);
            new Word("СПРЕЙ", "БРЫЗГИ", 0.5);
            new Word("СПРЕИ", "БРЫЗГИ", 0.5);
            new Word("ОРУЖИЕ", "ВИДЫ МАТЕРИАЛОВ", 0.5);
            new Word("ПУШКИ", "ВИДЫ МАТЕРИАЛОВ", 0.5);
            new Word("ФАСТФУД", "ВЫСТРЕЛ БУРГЕРА", 0.5);
            new Word("ФАСТФУД", "УСЧКОРЕННАЯ ПИША", 0.5);
            new Word("ФАСТФУД", "КУДАХТУЮШИИ", 0.5);
            new Word("АРЕСТ", "ЯЕЫРЕСТ", 0.5);
            new Word("МОТОР", "МОТРО(ЖЖЖЖЖЖЖЖЖЖ)", 0.5);
            new Word("ДВИГАТЕЛЬ", "ДВИЖАТЕЛЬ(ЗВУК МОТОРА)", 0.5);
            new Word("МОТОЦИКЛ", "ВЕЛОСИПЕД", 0.5);
            new Word("ВЕЛОСИПЕД", "БАИК", 0.5);
            new Word("ДРУГ", "homie", 0.2);
            new Word("ДРУГ", "СОБАКА", 0.2);
            new Word("БРАТ", "homie", 0.5);
            new Word("ПАССАЖИР", "ДРОБОВИК", 0.5);
            new Word("ЕБАТЬ", "ТРАХАНИЕ", 0.5);
            new Word("ЕБАНЫЙ", "ТРАХАНЫИ", 0.5);
            new Word("УХОДИ", "ПОЛУЧИ ТРАХАНИЕ", 0.5);
            new Word("ОТСЮДА", "ВНЕ ЗДЕСЬ", 0.5);
            new Word("ПОДЛИВКА", "ДОПОЛНИТЕЛЬНОЕ ПАДЕНИЕ", 0.5);
            new Word("ПОДЛИВКОЙ", "ДОПОЛНИТЕЛЬНЫМ ПАДЕНИЕМ", 0.5);
            new Word("ЕБАНЫЙ", "ТРАХАНЫИ", 0.5);
            new Word("РАССЛАБЬСЯ", "ОХЛАДИ ТРАХАНИЕ", 0.7);
            new Word("ШТАНЫ", "ПЕРЕЕХАННЫЕ ШТАНЫ", 0.9);
            new Word("БРЮКИ", "ПЕРЕЕХАННЫЕ ШТАНЫ", 0.8);
            new Word("ЕБАНЫЙ", "ТРАХАНЫИ", 0.9);
            new Word("ОБЩЕНИЕ", "КО-МИУ-НИИ-КАИ-ШУН", 0.4);
            new Word("МАЛЕНЬКИЙ", "small", 0.3);
            new Word("БОЛЬШ", "big", 0.1);
            new Word("БЫСТРЕЕ", "РАПИДО", 0.4);
            new Word("ДЫХАНИЕ", "(СОПЕНИЕ)", 0.4);
            new Word("ДОЗА", "КУСОК", 0.4);
            new Word("КОКС", "КОКС (ФЫРКАЮТ)", 0.4);
            new Word("ШКАЛА", "БРУСОК", 0.7);
            new Word("СТОЙ", "ОСТАНОВКА", 0.7);
            new Word("СТОЯТЬ", "ОСТАНОВКА", 0.7);
            new Word("ЗДОРОВЬЕ", "ЯДОРОВЬЕ", 0.5);
            new Word("РУЛЬ", "БАРАНКА", 0.5);
            new Word("РУЛЬ", "БАРАНКА (НАВЫКИ ВОЖДЕНИЯ УВЕЛИЧЕНЫ)", 0.5);
            new Word("БЫЛ", "ЫБЛ", 0.5);

            new RandomWord("НА УРОВНЕ ГРУНТА", 0.6);
            new RandomWord("esse?", 0.8);
            new RandomWord(", ТРАХАНЫИ,", 0.1);
            new RandomWord("(ФЫРКАЮТ)", 0.4);
            // new RandomWord("",0.01);
            // new Word("","",0.5);
        }
    }


    class Word
    {
        public static List<Word> Words = new List<Word>();

        public string Find, Repace;
        public double Prob;

        public Word(String Find, String Replace, double Prob)
        {
            this.Find = Find;
            this.Repace = Replace;
            this.Prob = Prob;
            Words.Add(this);
        }

        public static string Parse(String Arg)
        {
            foreach (Word word in Words)
            {
                if (Arg.Contains(word.Find))
                {
                    if (Wastificator.prob(word.Prob))
                    {
                        return Arg.Replace(word.Find, word.Repace);

                    }
                }
            }
            return Arg;
        }


    }

    class RandomWord
    {
        public static List<RandomWord> RandomWords = new List<RandomWord>();
        public string word;
        public double Prob;

        public RandomWord(String word, double Prob)
        {
            this.word = word;
            this.Prob = Prob;
            RandomWords.Add(this);
        }

        public static string TryPaste(String Arg)
        {
            if (Wastificator.prob(0.02))
            {
                foreach (RandomWord randomWord in RandomWords)
                {

                    if (Wastificator.prob(randomWord.Prob))
                    {
                        return Arg + " " + randomWord.word;

                    }
                }
            }

            
            return Arg;
        }
    }
}
