using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMgmt.Controllers
{
    [SessionAuth]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<IActionResult> Index(string searchTitle)
        {
            var books = string.IsNullOrEmpty(searchTitle)
                ? await _bookRepository.GetAllBooks() 
                : await _bookRepository.SearchBookByTitle(searchTitle);
            return View(books);
        }
        [HttpGet]
        [Route("book/details/{guid}")]
        public async Task<IActionResult> Details(Guid guid)
        {
            var book = await _bookRepository.GetBookById(guid);
            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid) 
            {
                await _bookRepository.AddBook(book);
                return RedirectToAction("Index", "Book");
            }
            return View(book);
        }
        [HttpGet]
        [Route("book/edit/{guid}")]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var book = await _bookRepository.GetBookById(guid);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.UpdateBook(book);
                return RedirectToAction("Index", "Book");
            }
            return View(book);
        }
        [HttpGet("book/delete/{guid}")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var book = await _bookRepository.GetBookById(guid);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid guid)
        {
            await _bookRepository.DeleteBook(guid);
            return RedirectToAction("Index", "Book");
        }

    }
}
