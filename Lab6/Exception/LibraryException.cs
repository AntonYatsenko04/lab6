using System;

namespace list
{
    public class LibraryException : Exception
    {
        public override string Message
        {
            get
            {
                return ErrorMessages.LibraryError;
            }
        } 
    }
    
}