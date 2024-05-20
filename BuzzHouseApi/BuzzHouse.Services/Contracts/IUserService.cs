using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IUserService
{ 
    Task<ServiceResult<User>> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> UpdateUserByIdAsync(Guid userId, User user);
    Task DeleteUserAsync(Guid userId);
}