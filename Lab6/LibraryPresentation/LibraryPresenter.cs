using list;

namespace Lab6;

public class LibraryPresenter
{
    private LibraryModel _libraryModel;
    private ILibraryView _libraryView;

    public LibraryPresenter(LibraryModel libraryModel, ILibraryView libraryView)
    {
        _libraryModel = libraryModel;
        _libraryView = libraryView;
    }

    public void ShowAllLibrary()
    {
        try
        {
            _libraryView.ShowLibraryTable(_libraryModel.GetAllLibraryData());
        }
        catch (FileParseException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (LibraryException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (NoAccessException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(ErrorMessages.GenericError + $": {e}");
        }
    }

    public void ShowLibraryItemsFilteredByName(string name)
    {
        try
        {
            _libraryView.ShowLibraryTable(_libraryModel.GetLibraryEntitiesWithName(name));
        }
        catch (FileParseException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (LibraryException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (NoAccessException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (Exception e)
        {
           _libraryView.ShowErrorMessage(ErrorMessages.GenericError + $": {e}");
        }
    }

    public void AddLibraryItem(LibraryEntity libraryEntity)
    {
        try
        {
            _libraryModel.InsertNewLibraryItem(libraryEntity);
            _libraryView.ShowMessage("Добавление произошло успешно");
        }
        catch (FileParseException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (LibraryException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (NoAccessException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(ErrorMessages.GenericError + $": {e}");
        }
    }

    public void DeleteLibraryItemByName(string name)
    {
        try
        {
            if (!_libraryModel.DeleteLibraryItemByName(name))
            {
                _libraryView.ShowMessage("Файл с указаным именем не найден");
            }
            else
            {
                _libraryView.ShowMessage("Файл удален успешно");
            }
            
        }
        catch (FileParseException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (LibraryException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (NoAccessException e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(ErrorMessages.GenericError + $": {e}");
        }
        
    }
}