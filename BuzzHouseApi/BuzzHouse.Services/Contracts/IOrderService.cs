using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IOrderService
{
    Task<ServiceResult<Order>> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersByCreatedDateAsync(DateTime createdDate);
    Task<IEnumerable<Order>> GetOrdersByOrderStatusAsync(int orderStatus);
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<Order> UpdateOrderByIdAsync(Guid orderId, Order order);
    Task DeleteOrderAsync(Guid orderId);
}