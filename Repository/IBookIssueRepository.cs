using LibraryMgmt.Models;
using LibraryMgmt.ViewModel;

namespace LibraryMgmt.Repository
{
    public interface IBookIssueRepository
    {
        void IssueBook(UserBookIssueViewModel userBookIssueViewModel);

        BookIssueListViewModel GetIssue(Guid guid);
        void ReturnBook(Guid guid);
        Task <IEnumerable<BookIssue>> GetIssuedBooks();
        IEnumerable<BookIssue> SearchIssuedBooks(string title);

        // Additional methods for fetching users and books
        IEnumerable<User> GetUsers();
        IEnumerable<Book> GetBooks();
    }
}
