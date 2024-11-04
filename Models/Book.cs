using System.ComponentModel.DataAnnotations;

namespace LibraryMgmt.Models
{
    public class Book
    {
        [Key]
        public Guid Guid { get; set; }= Guid.NewGuid();
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
    }
}
