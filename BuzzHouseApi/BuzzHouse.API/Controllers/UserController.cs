using BuzzHouse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.API.Controllers;

public class UserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("user")]  // Define the route for creating users
    public async Task<IActionResult> CreateUser([FromBody] User user) // Receive user data in the request body
    {
        var result = await _userService.CreateUserAsync(user);
        // Handle the result and return appropriate HTTP response (Created, BadRequest, etc.)
        return result;
    }
}