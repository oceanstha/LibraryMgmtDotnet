﻿using Humanizer;
using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
using LibraryMgmt.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LibraryMgmt.Controllers
{
    [SessionAuth]
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
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
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
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
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

        [Route("BookIssue/Subscribe/{bookId:Guid}")]
        public async Task<IActionResult> Subscribe(Guid bookId)
        {
            var adminUserId = HttpContext.Session.GetString("AdminUserId");
            Guid userId = Guid.Parse(adminUserId);
            (var userDetail, var userIssueDetail)  =await  _userRepository.GetUser(userId);
            bool isBookIssued = userIssueDetail.Any(issue => issue.BookId == bookId && issue.ReturnDate==null);
            if (isBookIssued)
            {
                TempData["SubscriptionExist"] = "You already have access to this book!";
            }else
            {
                var bookSubscribeViewModel = new UserBookIssueViewModel
                {
                    BookIssueId = Guid.NewGuid(),
                    SelectedBookId = bookId,
                    SelectedUserId = userId,
                };
                if (ModelState.IsValid)
                {
                    _bookIssueRepository.IssueBook(bookSubscribeViewModel);
                    TempData["SubsciptionSuccess"] = "You have access to the book now!";
                }
            }
            
            
            return RedirectToAction("Index", "Book");
        }


        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Index(string searchTitle)
        {
            
            var issueBooks = string.IsNullOrEmpty(searchTitle) ? await _bookIssueRepository.GetIssuedBooks() : _bookIssueRepository.SearchIssuedBooks(searchTitle);

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
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        [Route("BookIssue/Details/{id:Guid}")]
        public IActionResult Return(Guid id)
        {
            _bookIssueRepository.ReturnBook(id);
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy", AuthenticationSchemes = "CookieAuth")]
        [Route("BookIssue/Details/{id:Guid}")]
        public IActionResult Details(Guid id)
        {
            var BookIssueViewModel = _bookIssueRepository.GetIssue(id);
            return View(BookIssueViewModel);
        }
    }
}
