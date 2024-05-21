using BuzzHouse.Model.Models;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.API.Controllers;

[ApiController]
[Route("api/shoppingcart")]
public class ShoppingCartController: ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateShoppingCart([FromBody] ShoppingCart shoppingCart)
    {
        var result = await _shoppingCartService.CreateShoppingCartAsync(shoppingCart);

        if (result.HasErrors())
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Item);
    }

    [HttpGet]
    public async Task<IEnumerable<ShoppingCart>> GetShoppingCarts()
    {
        var result = await _shoppingCartService.GetShoppingCartsAsync();
        return result;
    }

    [HttpGet("{shoppingCartId}")]
    public async Task<ActionResult<ShoppingCart>> GetShoppingCartById(Guid shoppingCartId)
    {
        var result = await _shoppingCartService.GetShoppingCartByIdAsync(shoppingCartId);
       
        if (result == null)
            return NotFound();
        
        return result;
    }

    [HttpPut("{shoppingCartId}")]
    public async Task<ActionResult<ShoppingCart>> UpdateShoppingCartById(Guid shoppingCartId, ShoppingCart shoppingCart)
    {
        if (shoppingCart == null)
        {
            return BadRequest("Invalid shopping cart data");
        }
        
        var result = await _shoppingCartService.UpdateShoppingCartByIdAsync(shoppingCartId, shoppingCart);

        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpDelete("{shoppingCartId}")]
    public async Task<IActionResult> DeleteShoppingCart(Guid shoppingCartId)
    {
        await _shoppingCartService.DeleteShoppingCartAsync(shoppingCartId);
        return NoContent();
    }
}