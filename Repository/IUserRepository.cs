﻿using LibraryMgmt.Models;

namespace LibraryMgmt.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<(User, List<BookIssue>)> GetUser(Guid guid);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(Guid guid);
        string GetPdfFilePath(string fileName);

    }
}
