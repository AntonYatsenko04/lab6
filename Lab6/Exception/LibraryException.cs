using System;

namespace list
{
    public class LibraryException : Exception
    {
        public new string Message = ErrorMessages.LibraryError;
    }
    
}