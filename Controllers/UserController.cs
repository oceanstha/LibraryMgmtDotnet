using LibraryMgmt.Data;
using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMgmt.Controllers
{
    [SessionAuth]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //[Authorize(AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetUsers();
            return View(users);
        }

        [HttpGet]
        [Route("user/details/{guid}")]
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


        public IActionResult RenderPdf(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return NotFound("PDF file not found.");
            }
            // Ensure the path is properly formatted
            string trimmedPath = path.TrimStart('/');

            // Combine the base URL with the provided path
            string baseUrl = "https://localhost:7012";
            string fullUrl = $"{baseUrl}/{trimmedPath}";

            // Pass the full URL to the ViewBag
            ViewBag.PdfPath = fullUrl;
            return View();
        }

    }
}
