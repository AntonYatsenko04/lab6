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

    public void InsertNewLibraryItem(LibraryEntity libraryEntity)
    {
        _libraryRepository.AddLibraryEntity(libraryEntity);
    }

    public bool DeleteLibraryItemByName(string name)
    {
        return _libraryRepository.DeleteLibraryEntity(name);
    }
}