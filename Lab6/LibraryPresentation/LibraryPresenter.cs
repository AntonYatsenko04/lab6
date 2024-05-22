using System.Runtime.InteropServices;
using list;

namespace Lab6;

public class LibraryPresenter
{
    private LibraryModel _libraryModel;
    private ILibraryView _libraryView;
    private List<LibraryEntity> _libraryEntities;

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
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }

    public void ShowLibraryItemsFilteredByName(string name)
    {
        try
        {
            _libraryView.ShowLibraryTable(_libraryModel.GetLibraryEntitiesWithName(name));
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }
    
    public void ShowLibraryItemsFilteredByDateTime([Optional] DateTime? min,[Optional] DateTime? max)
    {
        try
        {
            
            _libraryView.ShowLibraryTable(_libraryModel.GetLibraryEntityFilteredByDateTime(min,max));
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }
    
    public void ShowLibraryItemsFilteredByFontSize([Optional]float? min,[Optional]float? max)
    {
        try
        {
            
            _libraryView.ShowLibraryTable(_libraryModel.GetLibraryEntityFilteredByFontSize(min,max));
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }
    
    public void ShowLibraryItemsFilteredByPageNumber([Optional]int? min,[Optional]int? max)
    {
        try
        {
            _libraryView.ShowLibraryTable(_libraryModel.GetLibraryEntityFilteredByPageNumber(min,max));
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }

    public void AddLibraryItem(LibraryEntity libraryEntity)
    {
        try
        {
            _libraryModel.InsertNewLibraryItem(libraryEntity);
            _libraryView.ShowMessage("Добавление произошло успешно");
        }
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
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
        catch (Exception e)
        {
            _libraryView.ShowErrorMessage(e.Message);
        }
    }
    
}