using System.Globalization;
using ConsoleTables;
using list;

namespace Lab6;

public partial  class LibraryView : ILibraryView
{
    private LibraryPresenter _libraryPresenter;

    public LibraryView()
    {
        _libraryPresenter = new LibraryPresenter(new LibraryModel(), this);
        _launchView();
    }

    private void _launchView()
    {
        while (true)
        {
            _log("Выберите операцию:");
            _log("1: Просмотр библиотеки");
            _log("2: Добавление элемента в библиотеку");
            _log("3: Удаление элемента из библиотеки");
            _log("4: Выбрать элементы с заданным именем");
            _log("5: Выбрать элементы с заданным диапазоном шрифта");
            _log("6: Выбрать элементы с заданным диапазоном номера страницы");
            _log("7: Выбрать элементы с заданным диапазоном даты создания");
            _log("8: Перейти в режим комбинированной выборки");
            _log("9: Окончить работу");

            string choice = Console.ReadLine();

            int choiceNum;

            if (int.TryParse(choice, out choiceNum))
            {
                switch (choiceNum)
                {
                    case 1:
                        _libraryPresenter.ShowAllLibrary();
                        break;
                    case 2:
                        _addLibraryItem();
                        break;
                    case 3:
                        _deleteLibraryItemByName();
                        break;
                    case 4:
                        _showLibraryItemsWithName();
                        break;
                    case 5:
                        _showLibraryItemsFilteredByFontSize();
                        break;
                    case 6:
                        _showLibraryItemsFilteredByPageNumber();
                        break;
                    case 7:
                        _showLibraryItemsFilteredByDateTime();
                        break;
                    case 8:
                        _showLibraryViewCombination();
                        break;
                    case 9:
                        return;
                }
            }
        }
    }

    private void _addLibraryItem()
    {
        _log("Введите имя файла");
        string fileName;
        if (!_readNotEmptyTrimmedString(out fileName))
        {
            return;
        }

        _log("Введите размер шрифта");
        if (!_readFontSize(out float fontSize))
        {
            return;
        }
        
        _log("Введите номер страницы");
        if (!_readPageNumber(out int pageNumber))
        {
            return;
        }

        LibraryEntity libraryEntity = new LibraryEntity(filePath: fileName, pageNumber: pageNumber, fontSize: fontSize);
        _libraryPresenter.AddLibraryItem(libraryEntity);
    }

    private void _deleteLibraryItemByName()
    {
        _log("Введите название файла");
        if (!_readNotEmptyTrimmedString(out string itemName))
        {
            return;
        }

        _libraryPresenter.DeleteLibraryItemByName(itemName);
    }

    

    private void _log(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowLibraryTable(List<LibraryEntity> libraryEntities)
    {
        if (_canShowTable)
        {
            var table = new ConsoleTable("Id", "Время", "Файл", "Размер шрифта", "Страница");
            foreach (var libraryEntity in libraryEntities)
            {
                string id = (libraryEntity.Id >= 0 ? libraryEntity.Id.ToString() : ErrorMessages.InvalidId) ?? ErrorMessages.InvalidId;
                string dateTime = (libraryEntity.DateTime <= DateTime.Now? libraryEntity.DateTime.ToString() : ErrorMessages.WrongDateTime) ?? ErrorMessages.WrongDateTime;
                string pageNumber = libraryEntity.PageNumber >= 1? libraryEntity.PageNumber.ToString() :ErrorMessages.WrongPageNumber;
                string fontSize = libraryEntity.FontSize >= 0? libraryEntity.FontSize.ToString(CultureInfo.InvariantCulture) :ErrorMessages.WrongPageNumber;
            
                table.AddRow(id, dateTime, libraryEntity.FilePath,
                    fontSize, pageNumber);
            }
        
            table.Write();
            Console.WriteLine();
        }
        
    }

    public void ShowErrorMessage(string message)
    {
        _log($"Ошибка: {message}");
    }

    public void ShowMessage(string message)
    {
        _log($"Сообщение: {message}");
    }
}