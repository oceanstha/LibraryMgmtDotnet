using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _libraryDbContext;

        public UserRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public async Task<User> AddUser(User user)
        {
            await _libraryDbContext.Users.AddAsync(user);
            await _libraryDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _libraryDbContext.Users.ToListAsync();
        }

        public async Task<User> GetUser(Guid guid)
        {
            
            return await _libraryDbContext.Users.FindAsync(guid);
        }
      

        public async Task<User> UpdateUser(User user)
        {
            _libraryDbContext.Users.Update(user);
            await _libraryDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUser(Guid guid)
        {
            var user = await _libraryDbContext.Users.FindAsync(guid);
            if (user == null)
            {
                return null; 
            }
            _libraryDbContext.Users.Remove(user);
            await _libraryDbContext.SaveChangesAsync();
            return user;
        }
    }
}
