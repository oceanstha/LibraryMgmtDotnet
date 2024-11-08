using LibraryMgmt.Repository;
using LibraryMgmt.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace LibraryMgmt.Controllers
{
    public class BookIssueController : Controller
    {
        private readonly IBookIssueRepository _bookIssueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BookIssueController(IBookIssueRepository bookIssueRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookIssueRepository = bookIssueRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Issue()
        {
            var users = await _userRepository.GetUsers();
            var books = await _bookRepository.GetAllBooks();
       
            if (!users.Any() || !books.Any())
            {
              
                throw new InvalidOperationException("Users or Books data is missing.");
            }

            var model = new UserBookIssueViewModel
            {
                Users = users.ToList(),
                Books = books.ToList(),
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Issue(UserBookIssueViewModel userBookIssueViewModel)
        {
            if (ModelState.IsValid)
            {
                _bookIssueRepository.IssueBook(userBookIssueViewModel);
                return RedirectToAction("Index");
            }
            userBookIssueViewModel.Users=_bookIssueRepository.GetUsers().ToList();
            userBookIssueViewModel.Books=_bookIssueRepository.GetBooks().ToList();
            return View(userBookIssueViewModel);
        }
        public IActionResult Index(string searchTitle)
        {
            
            var issueBooks = searchTitle.IsNullOrEmpty() ? _bookIssueRepository.GetIssuedBooks() : _bookIssueRepository.SearchIssuedBooks(searchTitle);

            var issuedBookViewModels = issueBooks.Select(issue => new BookIssueListViewModel
                {
                    Guid = issue.Guid,
                    BookTitle = issue.Book.Title,
                    UserName = issue.User.Name,
                    IssueDate = issue.IssueDate,
                    DueDate = issue.DueDate,
                    ReturnDate = issue.ReturnDate,

                }).ToList();
            
            return View(issuedBookViewModels);
        }
        [HttpPost]
        [Route("BookIssue/Details/{id:Guid}")]
        public IActionResult Return(Guid id)
        {
            Console.WriteLine("GUID in controller: " + id);
            _bookIssueRepository.ReturnBook(id);
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        [Route("BookIssue/Details/{id:Guid}")]
        public IActionResult Details(Guid id)
        {
            var BookIssueViewModel = _bookIssueRepository.GetIssue(id);
            return View(BookIssueViewModel);
        }
    }
}
