using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> SearchBookByTitle(string title);
        Task<Book> GetBookById(Guid Guid);
        Task<Book> AddBook(Book book, IFormFile file);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(Guid Guid);
    }
}
