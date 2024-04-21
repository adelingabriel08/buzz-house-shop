using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.API.Controllers;

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
    public async Task<ActionResult<User>> GetUserById(Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
       
        if (result == null)
            return NotFound();
        
        return result;
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<User>> UpdateUserById(Guid userId, User user)
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
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}