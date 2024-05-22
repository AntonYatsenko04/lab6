using list;

namespace Lab6;

public class LibraryModel
{
    private LibraryRepository _libraryRepository = new LibraryRepository();

    public List<LibraryEntity> GetAllLibraryData()
    {
        return _libraryRepository.GetLibraryData();
    }

    public List<LibraryEntity> GetLibraryEntitiesWithName(string name)
    {
        List<LibraryEntity> allLibraryEntities = _libraryRepository.GetLibraryData();

        List<LibraryEntity> selectedLibraryEntities = new List<LibraryEntity>();
        foreach (LibraryEntity libraryEntity in allLibraryEntities)
        {
            if (libraryEntity.FilePath == name)
            {
                selectedLibraryEntities.Add(libraryEntity);
            }
        }

        return selectedLibraryEntities;
    }

    public List<LibraryEntity> GetLibraryEntityFilteredByDateTime(DateTime? min,DateTime? max)
    {
        List<LibraryEntity> allLibraryEntities = _libraryRepository.GetLibraryData();

        List<LibraryEntity> selectedLibraryEntities = new List<LibraryEntity>();
        foreach (LibraryEntity libraryEntity in allLibraryEntities)
        {
            if (libraryEntity.DateTime >= (min ?? DateTime.MinValue) &&
                libraryEntity.DateTime <= (max ?? DateTime.MaxValue))
            {
                selectedLibraryEntities.Add(libraryEntity);
            }
        }

        return selectedLibraryEntities;
    }
    
    public List<LibraryEntity> GetLibraryEntityFilteredByPageNumber(int? min,int? max)
    {
        List<LibraryEntity> allLibraryEntities = _libraryRepository.GetLibraryData();

        List<LibraryEntity> selectedLibraryEntities = new List<LibraryEntity>();
        foreach (LibraryEntity libraryEntity in allLibraryEntities)
        {
            if (libraryEntity.PageNumber >= (min ?? int.MinValue) &&
                libraryEntity.PageNumber <= (max ?? int.MaxValue))
            {
                selectedLibraryEntities.Add(libraryEntity);
            }
        }

        return selectedLibraryEntities;
    } 
    public List<LibraryEntity> GetLibraryEntityFilteredByFontSize(float? min,float? max)
    {
        List<LibraryEntity> allLibraryEntities = _libraryRepository.GetLibraryData();

        List<LibraryEntity> selectedLibraryEntities = new List<LibraryEntity>();
        foreach (LibraryEntity libraryEntity in allLibraryEntities)
        {
            if (libraryEntity.FontSize >= (min ?? float.MinValue) &&
                libraryEntity.FontSize <= (max ?? float.MaxValue))
            {
                selectedLibraryEntities.Add(libraryEntity);
            }
        }

        return selectedLibraryEntities;
    } 
    

    public void InsertNewLibraryItem(LibraryEntity libraryEntity)
    {
        _libraryRepository.AddLibraryEntity(libraryEntity);
    }

    public bool DeleteLibraryItemByName(string name)
    {
        return _libraryRepository.DeleteLibraryEntity(name);
    }
}