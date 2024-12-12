using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IAdminUserRepository
    {
        Task AddAdminUserAsync(AdminUser adminUser);
        Task<IEnumerable<AdminUser>> GetAllAdminAsync();

    }
}
