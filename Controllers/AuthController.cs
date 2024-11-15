using LibraryMgmt.Data;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMgmt.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository) 
        {
            _authRepository=authRepository;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password) 
        {
            var adminUser = await _authRepository.AuthenticateUserAsync(username, password);
            if (adminUser == null)
            {
                ViewBag.Error = "Invalid Username or Password";
                return ViewBag.Error;
            }
            HttpContext.Session.SetString("AdminUserId", adminUser.Guid.ToString());
            return RedirectToAction("Index","Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }

    }
}
