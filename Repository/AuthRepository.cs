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
        public async Task<AdminUser> AuthenticateUserAsync(string Name, string PasswordHash)
        {
            var user = await _libraryDbContext.AdminUsers.FirstOrDefaultAsync(u=>u.Name == Name);
            //Console.WriteLine("USER" + user);
            //Console.WriteLine("PASSWORD"+BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
            if (user == null || !BCrypt.Net.BCrypt.Verify(PasswordHash, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        

        
    }
}
