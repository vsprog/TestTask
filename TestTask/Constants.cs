namespace TestTask
{
    /// <summary>
    /// Содержит различные константы приложения.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Регулярное выражение для русскоязычных гласных.
        /// </summary>
        public const string VowelPattern = @"[аоуэиыеёяю]";

        /// <summary>
        /// Регулярное выражение для русскоязычных согласных.
        /// </summary>
        public const string ConsonantsPattern = @"[йцкнгшщзхфвпрлджбтмсч]";

        /// <summary>
        /// Регулярное выражение для кириллицы.
        /// </summary>
        public const string CyrillicPattern = @"\p{IsCyrillic}";

        /// <summary>
        /// Регулярное выражение для дубликатов символов.
        /// </summary>
        public const string DuplicatePattern = @"(\w)\1";
    }
}
