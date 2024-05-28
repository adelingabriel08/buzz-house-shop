using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IUserService
{ 
    Task<ServiceResult<User>> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(string userId);
    Task<User> UpdateUserByIdAsync(string userId, User user);
    Task DeleteUserAsync(string userId);
    Task<User> GetOrCreateCurrentUserAsync();
}