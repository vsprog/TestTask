using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            using (IReadOnlyStream inputStream1 = GetInputStream(args[0]))
            using (IReadOnlyStream inputStream2 = GetInputStream(args[1]))
            {
                IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
                IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);
                    
                RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
                RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

                PrintStatistic(singleLetterStats);
                PrintStatistic(doubleLetterStats);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            var buffer = GetCyrillicCharsFromStream(stream);

            var result = buffer
                .GroupBy(c => c)
                .Select(g => new LetterStats
                {
                    Letter = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {            
            var buffer = GetCyrillicCharsFromStream(stream);
            var text = new string(buffer.ToArray());
            
            var result = Regex.Matches(text, Constants.DuplicatePattern, RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Select(m => m.Value.ToLower())
                .GroupBy(s => s)
                .Select(g => new LetterStats
                {
                    Letter = g.Key,
                    Count = g.Count()
                })
                .ToList();                

            return result;
        }

        /// <summary>
        /// Посимвольно читает поток.
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Список симовлов.</returns>
        private static IList<char> GetCyrillicCharsFromStream(IReadOnlyStream stream)
        {
            var buffer = new List<char>();

            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char current = stream.ReadNextChar();

                if (!Regex.IsMatch(current.ToString(), Constants.CyrillicPattern))
                {
                    continue;
                }

                buffer.Add(current);
            }

            return buffer;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            (letters as List<LetterStats>).RemoveAll(x => x.LetterType == charType);
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            var commonCounter = 0;

            foreach (var stat in letters.OrderBy(x => x.Letter))
            {
                commonCounter += stat.Count;
                Console.WriteLine($"{stat.Letter} : {stat.Count}");
            }

            Console.WriteLine($"ИТОГО : {commonCounter}");
        }
    }
}
