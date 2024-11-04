using System.ComponentModel.DataAnnotations;

namespace LibraryMgmt.Models
{
    public class BookIssue
    {
        [Key]
        public Guid guid { get; set; } // Primary key
        public Guid BookId { get; set; } // Foreign key to Book
        public Guid UserId { get; set; } // Foreign key to User
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable for books not yet returned

        public virtual Book Book { get; set; } // Navigation property
        public virtual User User { get; set; } // Navigation property
    }
}
