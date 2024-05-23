using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IShoppingCartService
{
    Task<ServiceResult<ShoppingCart>> CreateShoppingCartAsync(ShoppingCart shoppingCart);
    Task<ShoppingCart> AddCartItemToShoppingCartAsync(Guid shoppingCartId, CartItem cartItem);
    Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync();
    Task<ShoppingCart> GetShoppingCartByIdAsync(Guid shoppingCartId);
    Task<ShoppingCart> UpdateShoppingCartByIdAsync(Guid shoppingCartId, ShoppingCart shoppingCart);
    Task DeleteShoppingCartAsync(Guid shoppingCartId);
}