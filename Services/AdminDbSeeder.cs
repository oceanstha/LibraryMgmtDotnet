using BCrypt.Net;
using LibraryMgmt.Models;
using LibraryMgmt.Repository;

namespace LibraryMgmt.Services
{
    public class AdminDbSeeder
    {
        private readonly IAdminUserRepository _adminUserRespository;
        public AdminDbSeeder(IAdminUserRepository adminUserRepository)
        {
            _adminUserRespository = adminUserRepository;
        }
        public async Task SeedAdminUserAsync()
        {
            var adminUsers = await _adminUserRespository.GetAllAdminAsync();
            if (!adminUsers.Any())
            {
                var adminUser = new AdminUser
                {
                    Name = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin@123"),
                    Email = "adminUser@admin.com"
                };
                await _adminUserRespository.AddAdminUserAsync(adminUser);
            }
        }
    }
}
