using System.Text.RegularExpressions;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public struct LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Тип буквы или пары букв.
        /// </summary>
        public CharType LetterType 
        {
            get
            {
                if (Regex.IsMatch(Letter, Constants.VowelPattern, RegexOptions.IgnoreCase))
                {
                    return CharType.Vowel;
                }

                if (Regex.IsMatch(Letter, Constants.ConsonantsPattern, RegexOptions.IgnoreCase))
                {
                    return CharType.Consonants;
                }

                return CharType.Unknown;
            }
        }
    }
}
