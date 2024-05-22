using System.Runtime.InteropServices.JavaScript;
using list;

namespace Lab6;

public partial class LibraryView
{
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
            _log(ErrorMessages.EmptyStringError);
            result = "";
            return false;
        }
    }
    
    private bool _readDateTime(out DateTime dateTime)
    {
        if (_readNotEmptyTrimmedString(out string input))
        {
            if (DateTime.TryParse(input,out dateTime))
            {
                if (dateTime <= DateTime.Now && dateTime > DateTime.MinValue)
                {
                    return true;
                }
            }
            else
            {
                _log(ErrorMessages.UnableToParseInput);
                dateTime = DateTime.MinValue;
                return false;
            }
        }
        
        _log(ErrorMessages.WrongDateTime);
        dateTime = DateTime.MinValue;
        return false;
    }

    private bool _readFontSize(out float output)
    {
        if (_readNotEmptyTrimmedString(out string input))
        {
            input = input.Replace('.', ',');
            if (float.TryParse(input,out output))
            {
                if (output >= 0)
                {
                    return true;
                }
            }
            else
            {
                _log(ErrorMessages.UnableToParseInput);
                output = 0;
                return false;
            }
        }
        _log(ErrorMessages.WrongFontSize);
        output = 0;
        return false;
    }
    
    private bool _readPageNumber(out int output)
    {
        if (_readNotEmptyTrimmedString(out string input))
        {
            if (int.TryParse(input,out output))
            {
                if (output > 0)
                {
                    return true;
                }
            }
            else
            {
                _log(ErrorMessages.UnableToParseInput);
                output = 0;
                return false;
            }
        }
        _log(ErrorMessages.WrongPageNumber);
        output = 0;
        return false;
    }
    
    private bool _readSelection(out int output)
    {
        if (_readNotEmptyTrimmedString(out string input))
        {
            if (int.TryParse(input,out output))
            {
                if (output is > 0 and < 4)
                {
                    return true;
                }
            }
        }
        _log(ErrorMessages.WrongSelectionNumber);
        output = 0;
        return false;
    }
}