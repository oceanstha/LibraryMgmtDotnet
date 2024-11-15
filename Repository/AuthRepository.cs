using BCrypt.Net;
using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        

        public AuthRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public async Task<AdminUser> AuthenticateUserAsync(string username, string password)
        {
            var user = await _libraryDbContext.AdminUsers.FirstOrDefaultAsync(u=>u.Name == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        

        
    }
}
