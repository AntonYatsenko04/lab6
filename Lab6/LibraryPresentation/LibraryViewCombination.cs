namespace Lab6;

public partial class LibraryView
{
    private bool _canShowTable = true;
    private void _showLibraryViewCombination()
    {
        _canShowTable = false;
        _libraryPresenter.ClearStoredTable();
        
        while (true)
        {
            _log("Выберите операцию:");
            _log("1: Выбрать элементы с заданным именем");
            _log("2: Выбрать элементы с заданным диапазоном шрифта");
            _log("3: Выбрать элементы с заданным диапазоном номера страницы");
            _log("4: Выбрать элементы с заданным диапазоном даты создания");
            _log("5: Окончить фильтрацию и показать результат");

            string choice = Console.ReadLine();

            int choiceNum;

            if (int.TryParse(choice, out choiceNum))
            {
                switch (choiceNum)
                {
                    case 1:
                        _showLibraryItemsWithName();
                        break;
                    case 2:
                        _showLibraryItemsFilteredByFontSize();
                        break;
                    case 3:
                        _showLibraryItemsFilteredByPageNumber();
                        break;
                    case 4:
                        _showLibraryItemsFilteredByDateTime();
                        break;
                    case 5:
                        _canShowTable = true;
                        _libraryPresenter.ShowStoredTable();
                        return;
                        break;
                }
            }
        }
    }
}