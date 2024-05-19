using System;

namespace list
{
    public class LibraryEntity
    {
        public int PageNumber { get; set; }
        public float FontSize { get; set; }
        public string FilePath { get; set; }

        public LibraryEntity(int pageNumber, float fontSize, string filePath)
        {
            PageNumber = pageNumber;
            FontSize = fontSize;
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
    }
}