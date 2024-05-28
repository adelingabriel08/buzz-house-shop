using BuzzHouse.Model.Models;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.API.Controllers;

[Authorize]
[ApiController]
[Route("api/shoppingcart")]
public class ShoppingCartController: ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IProductService _productService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {   
        _shoppingCartService = shoppingCartService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateShoppingCart([FromBody] Guid userId)
    {
        var result = await _shoppingCartService.CreateShoppingCartAsync(userId);

        if (result.HasErrors())
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Item);
    }
    
    [HttpPost("{shoppingCartId}/items")]
    public async Task<IActionResult> AddCartItem(Guid shoppingCartId, [FromBody] CartItem cartItem)
    {
        try
        {
            var updatedCart = await _shoppingCartService.AddCartItemToShoppingCartAsync(shoppingCartId, cartItem);
            return Ok(updatedCart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{shoppingCartId}/items")]
    public async Task<IActionResult> UpdateCartItem(Guid shoppingCartId, [FromBody] CartItem cartItem)
    {
        try
        {
            var updatedCart = await _shoppingCartService.UpdateCartItemInShoppingCartAsync(shoppingCartId, cartItem);
            return Ok(updatedCart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{shoppingCartId}/items/{productId}")]
    public async Task<IActionResult> RemoveCartItem(Guid shoppingCartId, Guid productId)
    {
        try
        {
            var updatedCart = await _shoppingCartService.RemoveCartItemFromShoppingCartAsync(shoppingCartId, productId);
            return Ok(updatedCart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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