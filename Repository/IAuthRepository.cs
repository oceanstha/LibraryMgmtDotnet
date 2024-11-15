using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IAuthRepository
    {
        Task<AdminUser> AuthenticateUserAsync(string username, string password);
        
    }
}
