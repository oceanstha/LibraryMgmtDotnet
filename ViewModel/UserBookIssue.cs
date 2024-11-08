using LibraryMgmt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryMgmt.ViewModel
{
    public class UserBookIssueViewModel    {
        public Guid? BookIssueId { get; set; }
        public Guid SelectedBookId { get; set; } // To hold the selected book ID
        public Guid SelectedUserId { get; set; } // To hold the selected user ID
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime DueDate {  get; set; } = DateTime.Now.AddDays(14);
        public DateTime? ReturnDate { get; set; }
        public List<User> Users { get; set; } =new List<User>();
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
