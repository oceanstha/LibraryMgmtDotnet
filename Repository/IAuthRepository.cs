using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IAuthRepository
    {
        Task<User> AuthenticateUserAsync(string username, string password);
        
    }
}
