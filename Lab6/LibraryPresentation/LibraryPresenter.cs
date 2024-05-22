using System.Runtime.InteropServices;
using list;

namespace Lab6;

public class LibraryPresenter
{
    private LibraryModel _libraryModel;
    private ILibraryView _libraryView;
    private List<LibraryEntity> _storedLibraryEntities = new List<LibraryEntity>();

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
            var libraryEntites = _libraryModel.GetLibraryEntitiesWithName(name);
            _addToStorage(libraryEntites);
            _libraryView.ShowLibraryTable(libraryEntites);
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
            var libraryEntites = _libraryModel.GetLibraryEntityFilteredByDateTime(min, max);
            _addToStorage(libraryEntites);
            _libraryView.ShowLibraryTable(libraryEntites);
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
            var libraryEntites = _libraryModel.GetLibraryEntityFilteredByFontSize(min,max);
            _addToStorage(libraryEntites);
            _libraryView.ShowLibraryTable(libraryEntites);
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
            var libraryEntites = _libraryModel.GetLibraryEntityFilteredByPageNumber(min,max);
            _addToStorage(libraryEntites);
            _libraryView.ShowLibraryTable(libraryEntites);
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

    public void ClearStoredTable()
    {
        _storedLibraryEntities.Clear();
    }

    public void ShowStoredTable()
    {
        _libraryView.ShowLibraryTable(_storedLibraryEntities);
    }

    private void _addToStorage(List<LibraryEntity> libraryEntities)
    {
        List<LibraryEntity> newStoredLibraryEntites = new List<LibraryEntity>();

        if (_storedLibraryEntities.Count == 0)
        {
            _storedLibraryEntities.AddRange(libraryEntities);
            return;
        }
        foreach (var libraryEntity in libraryEntities)
        {
            foreach (LibraryEntity storedLibraryEntity in _storedLibraryEntities)
            {
                if (libraryEntity.Id == storedLibraryEntity.Id)
                {
                    newStoredLibraryEntites.Add(libraryEntity);
                }
               
            }
        }
        
        _storedLibraryEntities.Clear();
        _storedLibraryEntities.AddRange(newStoredLibraryEntites);
    }
}