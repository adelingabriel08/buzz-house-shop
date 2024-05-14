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
    
    public async Task<ServiceResult<ShoppingCart>> CreateShoppingCartAsync(ShoppingCart shoppingCart)
    {
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

    public async Task<ShoppingCart> GetShoppingCartByIdAsync(Guid shoppingCartId)
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

    public async Task<ShoppingCart> UpdateShoppingCartByIdAsync(Guid shoppingCartId, ShoppingCart shoppingCart)
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
    
    public async Task DeleteShoppingCartAsync(Guid shoppingCartId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName, _shoppingCartOptions.ContainerName);
        await container.DeleteItemAsync<Product>(shoppingCartId.ToString(), new PartitionKey(shoppingCartId.ToString()));
    }
}