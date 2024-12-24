using LibraryMgmt.Data;
using LibraryMgmt.Models;
using LibraryMgmt.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IPdfService _pdfService;

        public UserRepository(LibraryDbContext libraryDbContext, IPdfService pdfService)
        {
            _libraryDbContext = libraryDbContext;
            _pdfService = pdfService;
        }

        public async Task<User> AddUser(User user)
        {
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }
            await _libraryDbContext.Users.AddAsync(user);
            await _libraryDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _libraryDbContext.Users.ToListAsync();
        }

        public async Task<(User,List<BookIssue>)> GetUser(Guid guid)
        {          
           var  userDetail =  await _libraryDbContext.Users.FindAsync(guid);
           var  userIssueDetail = await _libraryDbContext.BookIssues.Include(b=>b.Book).Include(u=>u.User).Where(u=>u.UserId == guid).ToListAsync();
           return (userDetail, userIssueDetail);
        }
      

        public async Task<User> UpdateUser(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
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
        public string GetPdfFilePath(string fileName)
        {
            // You can add additional business logic here before accessing the service
            return _pdfService.GetPdfFile(fileName);
        }
    }
}
