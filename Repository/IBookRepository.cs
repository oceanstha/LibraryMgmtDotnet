using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(Guid Guid);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(Guid Guid);
    }
}
