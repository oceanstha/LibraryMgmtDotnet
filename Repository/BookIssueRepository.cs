using LibraryMgmt.Data;
using LibraryMgmt.Models;
using LibraryMgmt.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class BookIssueRepository : IBookIssueRepository
    {
        private readonly LibraryDbContext _libraryContext;
        
        public BookIssueRepository(LibraryDbContext libraryContext) {
            _libraryContext = libraryContext;  
        }

        public IEnumerable<BookIssue> GetIssuedBooks()
        {
            return _libraryContext.BookIssues.Include(b=>b.Book).Include(u=>u.User).ToList();
        }
        public IEnumerable<BookIssue> SearchIssuedBooks(string title)
        {
            return _libraryContext.BookIssues.Where(s=>s.Book.Title.Contains(title)).Include(b => b.Book).Include(u => u.User).ToList();
        }

        public BookIssueListViewModel GetIssue(Guid Guid)
        {
            BookIssue? IssueBook = _libraryContext.BookIssues.Include(b=>b.Book).Include(u=>u.User).FirstOrDefault(g=>g.Guid==Guid);
                              
            var BookIssueViewModel = new BookIssueListViewModel
            {
                Guid = IssueBook.Guid,
                BookTitle = IssueBook.Book.Title,
                UserName = IssueBook.User.Name,
                IssueDate = IssueBook.IssueDate,
                DueDate = IssueBook.DueDate,
                ReturnDate = IssueBook.ReturnDate,
            };
            return BookIssueViewModel;
        }

        public void IssueBook(UserBookIssueViewModel userBookIssueViewModel)
        {
            var bookIssue= new BookIssue
            {
                Guid = Guid.NewGuid(),
                BookId = userBookIssueViewModel.SelectedBookId,
                UserId = userBookIssueViewModel.SelectedUserId,
                IssueDate = userBookIssueViewModel.IssueDate,
                DueDate = userBookIssueViewModel.DueDate,
                ReturnDate = userBookIssueViewModel.ReturnDate,
            };
            _libraryContext.BookIssues.Add(bookIssue);
            _libraryContext.SaveChanges();
        }

        public void ReturnBook(Guid Guid)
        {
            Console.WriteLine("GUID in Repository: " + Guid);
            var bookIssue = _libraryContext.BookIssues.Find(Guid);
            if (bookIssue != null)
            {
                bookIssue.ReturnDate = DateTime.Now;
                _libraryContext.SaveChanges();
            }
        }
        public IEnumerable<Book> GetBooks()
        {
            return _libraryContext.Books.ToList();
        }
        public IEnumerable<User> GetUsers()
        {
            return _libraryContext.Users.ToList();
        }
    }
}
