using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class BookIssueRepository : IBookIssueRepository
    {
        private readonly LibraryDbContext _libraryContext;
        public BookIssueRepository(LibraryDbContext libraryContext) {
        _libraryContext = libraryContext;
        }

        public Task<BookIssue> GetIssue(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookIssue>> GetIssues()
        {
           return await _libraryContext.BookIssues.ToListAsync();
            
        }

        public Task<BookIssue> IssueBook(Guid BookId, Guid UserId)
        {
            throw new NotImplementedException();
        }
    }
}
