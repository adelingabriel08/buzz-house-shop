using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Services.Services;

public class ShoppingCartService: IShoppingCartService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly ShoppingCartOptions _shoppingCartOptions;
    
    public ShoppingCartService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<ShoppingCartOptions> shoppingCartOptions)
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = options.Value;
        _shoppingCartOptions = shoppingCartOptions.Value;
    }
    public async Task<ServiceResult<ShoppingCart>> CreateShoppingCartAsync(string userId)
    {
        ShoppingCart shoppingCart = new ShoppingCart();
        shoppingCart.Id = userId;
        shoppingCart.UserId = userId;
        shoppingCart.CartItems = new List<CartItem>();
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            await container.CreateItemAsync(shoppingCart, new PartitionKey(shoppingCart.Id.ToString()));
        }
        catch (Exception ex)
        {
            return ServiceResult<ShoppingCart>.Failed(ex.Message);
        }

        return ServiceResult<ShoppingCart>.Succeded(shoppingCart);
    }
    
    public async Task<ShoppingCart> AddCartItemToShoppingCartAsync(string shoppingCartId, CartItem cartItem)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var response = await container.ReadItemAsync<ShoppingCart>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));

            var shoppingCart = response.Resource;
            cartItem.Price = cartItem.Product.Price * cartItem.Quantity;

            shoppingCart.CartItems.Add(cartItem);

            var updatedResponse = await container.UpsertItemAsync(shoppingCart, new PartitionKey(shoppingCartId.ToString()));
            return updatedResponse.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new Exception("Shopping cart not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
     public async Task<ShoppingCart> UpdateCartItemInShoppingCartAsync(string shoppingCartId, CartItem updatedCartItem)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var response = await container.ReadItemAsync<ShoppingCart>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));

            var shoppingCart = response.Resource;
            var cartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.Product.Id == updatedCartItem.Product.Id);

            if (cartItem != null)
            {
                cartItem.Quantity = updatedCartItem.Quantity;
                cartItem.ProductSize = updatedCartItem.ProductSize;
                cartItem.CustomDetails = updatedCartItem.CustomDetails;
                cartItem.CustomImg = updatedCartItem.CustomImg;
                cartItem.Price = updatedCartItem.Price;

                var updatedResponse = await container.UpsertItemAsync(shoppingCart, new PartitionKey(shoppingCartId.ToString()));
                return updatedResponse.Resource;
            }
            else
            {
                throw new Exception("CartItem not found in ShoppingCart");
            }
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new Exception("Shopping cart not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<ShoppingCart> RemoveCartItemFromShoppingCartAsync(string shoppingCartId, Guid productId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var response = await container.ReadItemAsync<ShoppingCart>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));

            var shoppingCart = response.Resource;
            var cartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.Product.Id == productId);

            if (cartItem != null)
            {
                shoppingCart.CartItems.Remove(cartItem);

                var updatedResponse = await container.UpsertItemAsync(shoppingCart, new PartitionKey(shoppingCartId.ToString()));
                return updatedResponse.Resource;
            }
            else
            {
                throw new Exception("CartItem not found in ShoppingCart");
            }
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new Exception("Shopping cart not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync()
    {
        var shoppingCarts = new List<ShoppingCart>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var query = container.GetItemQueryIterator<ShoppingCart>(new QueryDefinition(
                "SELECT sc.id, sc.userId, sc.cartItems FROM " + _shoppingCartOptions.ContainerName +
                " AS sc"));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                shoppingCarts.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return shoppingCarts;
    }

    public async Task<ShoppingCart> GetShoppingCartByIdAsync(string shoppingCartId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var response = await container.ReadItemAsync<ShoppingCart>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ShoppingCart> UpdateShoppingCartByIdAsync(string shoppingCartId, ShoppingCart shoppingCart)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
            var response = await container.UpsertItemAsync(shoppingCart, new PartitionKey(shoppingCartId.ToString()));
            return response.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task DeleteShoppingCartAsync(string shoppingCartId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
        await container.DeleteItemAsync<Product>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));
    }
}