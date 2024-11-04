using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        public BookRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public async Task<Book> AddBook(Book book)
        {
            await _libraryDbContext.Books.AddAsync(book);
            await _libraryDbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book> DeleteBook(Guid Guid)
        {
            var book = await _libraryDbContext.Books.FindAsync(Guid);
            _libraryDbContext.Books.Remove(book);
            await _libraryDbContext.SaveChangesAsync();
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _libraryDbContext.Books.ToListAsync();
        }

        public async Task<Book> GetBookById(Guid Guid)
        {
            return await _libraryDbContext.Books.FindAsync(Guid);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _libraryDbContext.Books.Update(book);
            await _libraryDbContext.SaveChangesAsync();
            return book;
        }
    }
}
