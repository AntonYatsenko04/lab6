using System;

namespace list
{
    public class LibraryItemEntity
    {
        public int PageNumber { get; set; }
        public float FontSize { get; set; }
        public int BufferSize { get; set; }
        public string FilePath { get; set; }

        public LibraryItemEntity(int pageNumber, float fontSize, int bufferSize, string filePath)
        {
            PageNumber = pageNumber;
            FontSize = fontSize;
            BufferSize = bufferSize;
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
    }
}