using System.ComponentModel.DataAnnotations;

namespace LibraryMgmt.Models
{
    public class User
    {
        [Key]
        public Guid guid { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
