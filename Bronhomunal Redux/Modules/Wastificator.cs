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
        private static readonly bool Initialized = false;


        public static string Wastificate(string Arg)
        {
            if (!Initialized)
            {
                Initialize();
            }

            Rnd = new Random(1);
            Arg = Arg.ToUpper();

            string[] Words = Arg.Split(' ');
            string _Arg = "";
            foreach (string word in Words)
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
                    if (Prob(0.05))
                    {
                        _Char = 'Ы';
                    }
                }

                if (_Char == 'Д')
                {
                    if (Prob(0.005))
                    {
                        _Char = 'В';
                    }
                }

                if (_Char == 'S')
                {
                    if (Prob(0.9))
                    {
                        _Char = 'Ы';
                    }
                }

                if (_Char == 'Р')
                {
                    if (Prob(0.1))
                    {
                        _Char = 'Я';
                    }
                }

                if (_Char == 'Г')
                {
                    if (Prob(0.1))
                    {
                        _Char = 'Ж';
                    }
                }

                if (_Char == 'Е')
                {
                    if (Prob(0.03))
                    {
                        _Char = 'З';
                    }
                }

                if (_Char == 'П')
                {
                    if (Prob(0.01))
                    {
                        _Arg += "ТТ";
                        continue;
                    }
                }

                if (_Char == '.')
                {
                    if (Prob(0.01))
                    {
                        _Arg += ", esse?";
                        continue;
                    }
                }

                if (_Char == '.')
                {
                    if (Prob(0.01))
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

        private static bool Prob(double Prob)
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

        private class Word
        {
            private static readonly List<Word> _words = new List<Word>();
			private readonly string _find, _repace;
			private readonly double _prob;

            public Word(string find, string replace, double prob)
            {
                _find = find;
                _repace = replace;
                _prob = prob;
                _words.Add(this);
            }

            public static string Parse(string arg)
            {
                foreach (Word word in _words)
                {
                    if (arg.Contains(word._find))
                    {
                        if (Prob(word._prob))
                        {
                            return arg.Replace(word._find, word._repace);

                        }
                    }
                }
                return arg;
            }


        }

        private class RandomWord
        {
            private static readonly List<RandomWord> _randomWords = new List<RandomWord>();
            private readonly string _word;
            private readonly double _prob;

            public RandomWord(string word, double prob)
            {
                _word = word;
                _prob = prob;
                _randomWords.Add(this);
            }

            public static string TryPaste(string arg)
            {
                if (Prob(0.02))
                {
                    foreach (RandomWord randomWord in _randomWords)
                    {

                        if (Prob(randomWord._prob))
                        {
                            return arg + " " + randomWord._word;

                        }
                    }
                }

                return arg;
            }
        }
    }


    
}
