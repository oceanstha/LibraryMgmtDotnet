using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IBookIssueRepository
    {
        Task<BookIssue> GetIssue (Guid guid);
        Task<IEnumerable<BookIssue>> GetIssues ();
        Task<BookIssue> IssueBook(Guid BookId, Guid UserId);
    }
}
