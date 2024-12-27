using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryMgmt.Controllers
{
    [SessionAuth]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BookController(IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(string searchTitle)
        {
            var adminUserId = HttpContext.Session.GetString("AdminUserId");
            Guid userId = Guid.Parse(adminUserId);
            (var userDetail, var userIssueDetail) = await _userRepository.GetUser(userId);
            

            var books = string.IsNullOrEmpty(searchTitle)
                ? await _bookRepository.GetAllBooks() 
                : await _bookRepository.SearchBookByTitle(searchTitle);

            var bookViewModels = books.Select(book => new ViewModel.BookViewModel
            {
                Book = book,
                IsIssued = userIssueDetail.Any(issue => issue.BookId == book.Guid && issue.ReturnDate==null) 
            }).ToList();

            return View(bookViewModels);
            
        }

        [HttpGet]
        [Route("book/details/{guid}")]
        public async Task<IActionResult> Details(Guid guid)
        {
            var book = await _bookRepository.GetBookById(guid);
            return View(book);
        }

        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public  async Task<IActionResult> Create(Book book, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _bookRepository.AddBook(book, file);
                    TempData["SuccessMessage"] = "Book added successfully!";
                    return RedirectToAction("Index", "Book");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }
            return View(book);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        [Route("book/edit/{guid}")]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var book = await _bookRepository.GetBookById(guid);
            return View(book);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
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
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
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
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> DeleteConfirmed(Guid guid)
        {
            await _bookRepository.DeleteBook(guid);
            return RedirectToAction("Index", "Book");
        }
        


    }
}
