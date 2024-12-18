using LibraryMgmt.Data;
using LibraryMgmt.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Login(string Email, string PasswordHash) 
        {
            var user = await _authRepository.AuthenticateUserAsync(Email, PasswordHash);
            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View();
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Role", user.Role)
            };

            // Create the ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            // Create a ClaimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user using cookies
            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            HttpContext.Session.SetString("AdminUserId", user.guid.ToString());
            return RedirectToAction("Index","Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }

    }
}
