using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Services.Services;

public class OrderService: IOrderService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly OrdersOptions _ordersOptions;
    private readonly IShoppingCartService _shoppingCartService;
    public OrderService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<OrdersOptions> ordersOptions, IShoppingCartService shoppingCartService)
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = options.Value;
        _ordersOptions = ordersOptions.Value;
        _shoppingCartService = shoppingCartService;
    }
    
    public async Task<ServiceResult<Order>> CreateOrderAsync(Guid userId)
    {
        Order order = new Order();
        try
        {
            var shoppingCart = await _shoppingCartService.GetShoppingCartByIdAsync(userId);
            Console.Write(shoppingCart);
        
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found for user.");
            }

            //var order = new Order
            //{
            order.Id = Guid.NewGuid();
            order.CreatedDate = DateTime.UtcNow;
            order.DeliveryDate = DateTime.UtcNow.AddDays(3);
            order.UserId = userId;
            order.ShoppingCart = shoppingCart;
            order.OrderStatus = 0;
            order.ShippingAddress = new ShippingAddress();
            //};
            
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            await container.CreateItemAsync(order, new PartitionKey(order.UserId.ToString()));
        }
        catch (Exception ex)
        {
            return ServiceResult<Order>.Failed(ex.Message);
        }

        return ServiceResult<Order>.Succeded(order);
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var orders = new List<Order>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var query = container.GetItemQueryIterator<Order>(new QueryDefinition(
                "SELECT o.id, o.createdDate, o.deliveryName, o.userId, o.shippingAddress, o.orderStatus, o.shoppingCart FROM " + _ordersOptions.ContainerName +
                " AS o"));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                orders.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return orders;
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByCreatedDateDescAsync()
    {
        var orders = new List<Order>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var query = container.GetItemQueryIterator<Order>(new QueryDefinition(
                "SELECT o.id, o.createdDate, o.deliveryName, o.userId, o.shippingAddress, o.orderStatus, o.shoppingCart FROM " + _ordersOptions.ContainerName +
                " AS o ORDER BY o.createdDate DESC" ));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                orders.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return orders;
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByOrderStatusAsync(int orderStatus)
    {
        var orders = new List<Order>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var query = container.GetItemQueryIterator<Order>(new QueryDefinition(
                "SELECT o.id, o.createdDate, o.deliveryName, o.userId, o.shippingAddress, o.orderStatus, o.shoppingCart FROM " + _ordersOptions.ContainerName +
                " AS o WHERE o.orderStatus = " + orderStatus));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                orders.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return orders;
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var response = await container.ReadItemAsync<Order>(orderId.ToString(), new PartitionKey(orderId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Order> UpdateOrderByIdAsync(Guid orderId, Order order)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var response = await container.UpsertItemAsync(order, new PartitionKey(orderId.ToString()));
            return response.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task DeleteOrderAsync(Guid orderId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
        await container.DeleteItemAsync<Order>(orderId.ToString(), new PartitionKey(orderId.ToString()));
    }
}