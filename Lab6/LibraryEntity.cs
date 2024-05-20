using System;

namespace list
{
    public class LibraryEntity
    {
        public LibraryEntity(int id, DateTime dateTime, int pageNumber, float fontSize, string filePath)
        {
            Id = id;
            DateTime = dateTime;
            PageNumber = pageNumber;
            FontSize = fontSize;
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
        
        public LibraryEntity(int pageNumber, float fontSize, string filePath)
        {
            
            PageNumber = pageNumber;
            FontSize = fontSize;
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
        
        

        public int? Id{ get; set; }

        public DateTime? DateTime{ get; set; }
        public int PageNumber { get; set; }
        public float FontSize { get; set; }
        public string FilePath { get; set; }

       
    }
}