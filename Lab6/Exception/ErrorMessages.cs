namespace list
{
    public class ErrorMessages
    {
        public const string GenericError = "Возникла неизвестная ошибка.";
        public const string LibraryError = "Критическая ошибка библиотеки. Файл будет создан заново";
        public const string EmptyStringError = "Строка пуста. В следующий раз введите строку";
        public const string UnableToParseInput = "Невозможно преобразовать ввод в необходимый тип данных";
        public const string WrongDateTime = "Дата должна быть не более сегодняшней и не менее минимальной для данной системы";
        public const string WrongFontSize = "Размер шрифта должен быть не менее 0";
        public const string WrongPageNumber = "Номер страницы должен быть не менее 1";
        public const string InvalidId = "Неверный идентификатор";
        public const string WrongSelectionNumber = "Неверный ввод выбора";
        public const string MinCantBeMoreThanMax = "Меньшее не может быть больше большего";
        
    }
}