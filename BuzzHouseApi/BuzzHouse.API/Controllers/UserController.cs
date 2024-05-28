using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user) 
    {
        var result  = await _userService.CreateUserAsync(user);

        if (result.HasErrors())
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Item);
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        var result = await _userService.GetUsersAsync();
        return result;
    }

    [HttpGet("{userId}")]
    //[Route("getUserById")]
    public async Task<ActionResult<User>> GetUserById(string userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
       
        if (result == null)
            return NotFound();
        
        return result;
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<User>> UpdateUserById(string userId, User user)
    {
        if (user == null)
        {
            return BadRequest("Invalid user data");
        }
        
        var result = await _userService.UpdateUserByIdAsync(userId, user);

        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}