using BuzzHouse.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.API.Controllers;

[Authorize]
[ApiController]
[Route("api/dummy")]
public class DummyController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDummy()
    {
        return Ok(new SampleModel { Name = "Dummy" });
    }
}