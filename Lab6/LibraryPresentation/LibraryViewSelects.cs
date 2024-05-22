using list;

namespace Lab6;

public partial class LibraryView
{
    private void _showLibraryItemsWithName()
    {
        _log("Введите название файла");
        if (!_readNotEmptyTrimmedString(out string itemName))
        {
            return;
        }

        _libraryPresenter.ShowLibraryItemsFilteredByName(itemName);
    }

    private void _showLibraryItemsFilteredByDateTime()
    {
        _log(Messages.SelectionString);
        if (!_readSelection(out int selection))
        {
            return;
        }

        DateTime min = DateTime.MinValue;
        DateTime max = DateTime.MaxValue;
        if (selection is 1 or 3)
        {
            _log("Введите время Мин в формате дд.мм.гггг чч:мм:сс");
            if (!_readDateTime(out min))
            {
                return;
            }
            
        }

        if (selection is 2 or 3)
        {
            _log("Введите время Макс в формате дд.мм.гггг чч:мм:сс");
            if (!_readDateTime(out max))
            {
                return;
            }
        }

        if (min>max)
        {
            _log(ErrorMessages.MinCantBeMoreThanMax);
            return;
        }

        switch (selection)
        {
            case 1:
                _libraryPresenter.ShowLibraryItemsFilteredByDateTime(min: min);
                break;
            case 2:
                _libraryPresenter.ShowLibraryItemsFilteredByDateTime(max: max);
                break;
            case 3:
                _libraryPresenter.ShowLibraryItemsFilteredByDateTime(min: min,max: max);
                break;
        }
    }
    
    private void _showLibraryItemsFilteredByFontSize()
    {
        _log(Messages.SelectionString);
        if (!_readSelection(out int selection))
        {
            return;
        }

        float min = float.MinValue;
        float max = float.MaxValue;
        if (selection is 1 or 3)
        {
            _log("Введите размер шрифта Мин");
            if (! _readFontSize(out min))
            {
                return;
            }
        }

        if (selection is 2 or 3)
        {
            _log("Введите размер шрифта Макс");
            if (! _readFontSize(out max))
            {
                return;
            }
           
        }

        if (min>max)
        {
            _log(ErrorMessages.MinCantBeMoreThanMax);
            return;
        }

        switch (selection)
        {
            case 1:
                _libraryPresenter.ShowLibraryItemsFilteredByFontSize(min: min);
                break;
            case 2:
                _libraryPresenter.ShowLibraryItemsFilteredByFontSize(max: max);
                break;
            case 3:
                _libraryPresenter.ShowLibraryItemsFilteredByFontSize(min: min,max: max);
                break;
        }
    }
    
    private void _showLibraryItemsFilteredByPageNumber()
    {
        _log(Messages.SelectionString);
        if (!_readSelection(out int selection))
        {
            return;
        }

        int min = int.MinValue;
        int max = int.MaxValue;
        if (selection is 1 or 3)
        {
            _log("Введите номер страницы Мин");
            if (! _readPageNumber(out min))
            {
                return;
            }
            
        }

        if (selection is 2 or 3)
        {
            _log("Введите номер страницы Макс");
            if (! _readPageNumber(out max))
            {
                return;
            }
          
        }

        if (min>max)
        {
            _log(ErrorMessages.MinCantBeMoreThanMax);
            return;
        }

        switch (selection)
        {
            case 1:
                _libraryPresenter.ShowLibraryItemsFilteredByPageNumber(min: min);
                break;
            case 2:
                _libraryPresenter.ShowLibraryItemsFilteredByPageNumber(max: max);
                break;
            case 3:
                _libraryPresenter.ShowLibraryItemsFilteredByPageNumber(min: min,max: max);
                break;
        }
    }
}