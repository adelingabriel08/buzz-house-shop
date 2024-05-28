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
    private readonly IEmailSender _emailSender;
    private readonly IUserService _userService;

    public OrderService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<OrdersOptions> ordersOptions, IEmailSender emailSender, IUserService userService, IShoppingCartService shoppingCartService)
    {
        _cosmosClient = cosmosClient;
        _emailSender = emailSender;
        _userService = userService;
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
        var user = await _userService.GetOrCreateCurrentUserAsync();
        var orders = new List<Order>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
            var query = container.GetItemQueryIterator<Order>(new QueryDefinition(
                "SELECT o.id, o.createdDate, o.deliveryName, o.userId, o.shippingAddress, o.orderStatus, o.shoppingCart FROM " + _ordersOptions.ContainerName +
                " AS o WHERE o.userId = @userId").WithParameter("@userId", user.Id));

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

    public async Task HandleOrderChange(Order order)
    {
        var user = await _userService.GetUserByIdAsync(order.UserId);
        if (user is null)
        {
            return;
        }

        await _emailSender.SendEmailAsync(user.Id, "Buzz House - Your order has changed",
            $"<h4>Your order has been changed to the following details:</h4><br/>" +
            "<p><b>Order ID: </b>" + order.Id + "</p>" +
            "<p><b>Order Status: </b>" + order.OrderStatus + "</p>" +
            "<p><b>ShippingAddress Date: </b>" + order.ShippingAddress + "</p>" +
            "<p><b>Order Date: </b>" + order.CreatedDate + "</p>" +
            "<p><b>Cart:</p></b>" +
            "<ul>" +  string.Join("", order.ShoppingCart.CartItems.Select(x => string.Format("<li>{0}</li>", x.Product.Name))) + "</ul>");
    }
    
    public async Task DeleteOrderAsync(Guid orderId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_ordersOptions.ContainerName);
        await container.DeleteItemAsync<Order>(orderId.ToString(), new PartitionKey(orderId.ToString()));
    }
}