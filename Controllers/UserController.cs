using LibraryMgmt.Data;
using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryMgmt.Controllers
{
    [SessionAuth]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookIssueRepository _bookIssueRepository; 

        public UserController(IUserRepository userRepository, IBookIssueRepository bookIssueRepository)
        {
            _userRepository = userRepository;
            _bookIssueRepository = bookIssueRepository;
        }
        //[Authorize(AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetUsers();
            return View(users);
        }

        [HttpGet]
        [Route("user/details/{guid}")]
        [Authorize(AuthenticationSchemes ="CookieAuth")]
        public async Task<IActionResult> Details(Guid guid)
        {
            var user =await _userRepository.GetUser(guid);
            return View(user);
        }


        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Create(User user)
        {
            if ( ModelState.IsValid)
            {
                await _userRepository.AddUser(user);
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        [Route("user/edit/{guid}")]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var (user,userIssue) = await _userRepository.GetUser(guid);
            return View(user);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.UpdateUser(user);
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }

        [HttpGet("user/delete/{guid}")]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var (user, userIssue) = await _userRepository.GetUser(guid);
            
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> DeleteConfirmed(Guid guid)
        {
            
            await _userRepository.DeleteUser(guid);
            return RedirectToAction("Index", "User");
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> RenderPdf(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return NotFound("PDF file not found.");
            }
            //Console.WriteLine("PATH: "+path);
            var adminUserId = HttpContext.Session.GetString("AdminUserId");
            //Console.WriteLine("ADMINUSERID: "+adminUserId);
            Guid guid = Guid.Parse(adminUserId);
            //var (user, userIssue) = await _userRepository.GetUser(guid);

            var allIssuedBooks = await _bookIssueRepository.GetIssuedBooks(); 
            var bookIssues = allIssuedBooks.Where(x => x.UserId == guid);

            bool pathExists = bookIssues.Any(issue =>
            !string.IsNullOrEmpty(issue.Book.FilePath) &&
            issue.Book.FilePath.Contains(path, StringComparison.OrdinalIgnoreCase)
);

            //foreach (var bookIssue in bookIssues)
            //{
            //    Console.WriteLine($"Book: {bookIssue.Book.Title}, FilePath: {bookIssue.Book.FilePath}");
            //}

            if (pathExists)
            {
                string trimmedPath = path.TrimStart('/');
                string baseUrl = "https://localhost:7012";
                string fullUrl = $"{baseUrl}/{trimmedPath}";
                ViewBag.PdfPath = fullUrl;
                return View();
            }
            else
            {
                TempData["NotFoundMessage"] = "You do not have access to this book.";
                return RedirectToAction("Index", "Book");
            }
            
        }
    }
}
