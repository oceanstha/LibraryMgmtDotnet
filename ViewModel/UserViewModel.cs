using LibraryMgmt.Models;

namespace LibraryMgmt.ViewModel
{
    public class UserViewModel
    {
        public Guid guid { get; set; } 
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public List<BookIssue> books { get; set; }
    }
}
