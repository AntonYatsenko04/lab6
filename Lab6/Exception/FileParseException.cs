
namespace Lab6;

public class FileParseException: Exception
{

    public override string Message
    {
        get
        {
            return "Ошибка преобразования файла в объект, возможно, файл поврежден";
        }
    } 

}