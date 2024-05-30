using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IShoppingCartService
{
    Task<ServiceResult<ShoppingCart>> CreateShoppingCartAsync(string userId);
    Task<ShoppingCart> GetShoppingCartsAsync();
    Task<ShoppingCart> GetShoppingCartByIdAsync(string shoppingCartId);
    Task<ShoppingCart> UpdateShoppingCartByIdAsync(string shoppingCartId, ShoppingCart shoppingCart);
    Task DeleteShoppingCartAsync(string shoppingCartId);
    Task<ShoppingCart> AddCartItemToShoppingCartAsync(string shoppingCartId, CartItem cartItem);
    Task<ShoppingCart> UpdateCartItemInShoppingCartAsync(string shoppingCartId, CartItem updatedCartItem);
    Task<ShoppingCart> RemoveCartItemFromShoppingCartAsync(string shoppingCartId, Guid productId);
}