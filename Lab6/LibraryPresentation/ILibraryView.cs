using list;

namespace Lab6;

public interface ILibraryView
{
    void ShowLibraryTable(List<LibraryEntity> libraryEntities);

    void ShowErrorMessage(string message);
    
    void ShowMessage(string message);
}