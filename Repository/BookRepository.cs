using LibraryMgmt.Data;
using LibraryMgmt.Models;
using LibraryMgmt.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IFileUploadService _fileUploadService;
        public BookRepository(LibraryDbContext libraryDbContext, IFileUploadService fileUploadService)
        {
            _libraryDbContext = libraryDbContext;
            _fileUploadService = fileUploadService;
        }

        public async Task<Book> AddBook(Book book, IFormFile file)
        {
            // Upload the file using the FileUploadService
            if (file != null && file.Length > 0)
            {
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "book-files");
                book.FilePath = uploadedFilePath; // Set the file path in the book entity
            }

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
        public async Task<IEnumerable<Book>> SearchBookByTitle(string title)
        {
            return await _libraryDbContext.Books.Where(b=>b.Title.Contains(title)).ToListAsync();
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
