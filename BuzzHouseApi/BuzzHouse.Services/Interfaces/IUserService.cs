using BuzzHouse.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.Services.Interfaces;

public interface IUserService
{ 
    Task<IActionResult> CreateUserAsync(User user);
}