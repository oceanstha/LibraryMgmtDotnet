using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        public AdminUserRepository(LibraryDbContext libraryDbContext) 
        { 
            _libraryDbContext = libraryDbContext;
        }
        

        public async Task AddAdminUserAsync(AdminUser adminUser)
        {
            adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminUser.PasswordHash);
            await _libraryDbContext.AdminUsers.AddAsync(adminUser);
            await _libraryDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdminUser>> GetAllAdminAsync()
        {
            return await _libraryDbContext.AdminUsers.ToListAsync();
        }
    }
}
