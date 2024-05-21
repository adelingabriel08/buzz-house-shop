using BuzzHouse.Model.Models;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.API.Controllers;
[Route("api/orders")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var result = await _orderService.CreateOrderAsync(order);

        if (result.HasErrors())
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Item);
    }
    
    [HttpGet]
    public async Task<IEnumerable<Order>> GetOrders()
    {
        var result = await _orderService.GetOrdersAsync();
        return result;
    }

    [HttpGet("createdDate")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCreatedDateDesc()
    {
        var result = await _orderService.GetOrdersByCreatedDateDescAsync();
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet("orderStatus/{orderStatus}")]
    public async Task<IEnumerable<Order>> GetOrdersByStatusId(int orderStatus)
    {
        var result = await _orderService.GetOrdersByOrderStatusAsync(orderStatus);

        return result;
    }

    [HttpGet("orderId/{orderId}")]
    public async Task<ActionResult<Order>> GetOrderById(Guid orderId)
    {
        var result = await _orderService.GetOrderByIdAsync(orderId);
        
        if (result == null)
            return NotFound();
        
        return result;
    }
    
    [HttpPut("{orderId}")]
    public async Task<ActionResult<Order>> UpdateOrderById(Guid orderId, [FromBody] Order order)
    {
        if (order == null)
        {
            return BadRequest("Invalid order data");
        }

        var result = await _orderService.UpdateOrderByIdAsync(orderId, order);

        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await _orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }
}