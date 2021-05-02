namespace TestTask
{
    /// <summary>
    /// Содержит различные константы приложения.
    /// </summary>
    public static class Constants
    {
        public const string VowelPattern = @"[аоуэиыеёяю]";

        public const string ConsonantsPattern = @"[йцкнгшщзхфвпрлджбтмсч]";
        
        public const string CyrillicPattern = @"\p{IsCyrillic}";
        
        public const string DuplicatePattern = @"(\w)\1";
    }
}
