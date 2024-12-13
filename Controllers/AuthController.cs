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
        public async Task<IActionResult> Login(string Name, string PasswordHash) 
        {
            var adminUser = await _authRepository.AuthenticateUserAsync(Name, PasswordHash);
            if (adminUser == null)
            {
                ViewBag.Error = "Invalid Username or Password";
                return View();
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
