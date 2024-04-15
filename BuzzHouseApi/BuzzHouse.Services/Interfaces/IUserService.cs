using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.Services.Interfaces;

public interface IUserService
{ 
    Task<ServiceResult<User>> CreateUserAsync(User user); //change type of Task
}