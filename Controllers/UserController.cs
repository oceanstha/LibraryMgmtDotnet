using LibraryMgmt.Data;
using LibraryMgmt.Filters;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
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
        [Route("user/edit/{guid}")]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var (user,userIssue) = await _userRepository.GetUser(guid);
            return View(user);
        }

        [HttpPost]
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
        public async Task<IActionResult> DeleteConfirmed(Guid guid)
        {
            
            await _userRepository.DeleteUser(guid);
            return RedirectToAction("Index", "User");
        }

    }
}
