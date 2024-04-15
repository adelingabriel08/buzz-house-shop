using BuzzHouse.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
}