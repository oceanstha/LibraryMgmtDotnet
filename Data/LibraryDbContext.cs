using LibraryMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }
        

    }
}
