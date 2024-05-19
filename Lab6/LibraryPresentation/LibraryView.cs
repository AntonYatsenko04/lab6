using System.Globalization;
using ConsoleTables;
using list;

namespace Lab6;

public class LibraryView : ILibraryView
{
    private LibraryPresenter _libraryPresenter;

    public LibraryView()
    {
        _libraryPresenter = new LibraryPresenter(new LibraryModel(),this);
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
            _log("5: Окончить работу");

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
            _log(ErrorMessages.EmptyStringError);
            return;
        }

        _log("Введите размер шрифта");
        float fontSize;
        if (!_readNotEmptyTrimmedString(out var fontSizeStr) || !float.TryParse(fontSizeStr, out fontSize))
        {
            _log("Строка неверна. В следующий раз введите дробное число");
            return;
        }

        _log("Введите номер страницы");
        int pageNumber;
        if (!_readNotEmptyTrimmedString(out var pageNumberStr) || !int.TryParse(pageNumberStr, out pageNumber))
        {
            _log("Строка неверна. В следующий раз введите целое число");
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
            _log(ErrorMessages.EmptyStringError);
            return;
        }
        
        _libraryPresenter.DeleteLibraryItemByName(itemName);
        
    }

    private void _showLibraryItemsWithName()
    {
        _log("Введите название файла");
        if (!_readNotEmptyTrimmedString(out string itemName))
        {
            _log(ErrorMessages.EmptyStringError);
            return;
        }
        
        _libraryPresenter.ShowLibraryItemsFilteredByName(itemName);
    }

    private void _log(string message)
    {
        Console.WriteLine(message);
    }

    private bool _readNotEmptyTrimmedString(out string result)
    {
        string? input = Console.ReadLine();
        if (input != null && input.Trim().Length > 0)
        {
            result = input.Trim();
            return true;
        }
        else
        {
            result = "";
            return false;
        }
    }

    public void ShowLibraryTable(List<LibraryEntity> libraryEntities)
    {
        var table = new ConsoleTable("Файл", "Размер шрифта", "Страница");
        foreach (var libraryEntity in libraryEntities)
        {
            table.AddRow(libraryEntity.FilePath, libraryEntity.FontSize.ToString().Trim(), libraryEntity.PageNumber.ToString().Trim());
        }
        table.Write();
        Console.WriteLine();
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