using System.Runtime.InteropServices.JavaScript;

namespace BuzzHouse.Model.Models;

public class Order
{
    public Guid Id { set; get; }
    public DateTime CreatedDate { set; get; }
    public DateTime DeliveryDate { set; get; }
    public string UserId { set; get; }
    public ShippingAddress ShippingAddress { set; get; }
    public string OrderStatus { set; get; }
    public ShoppingCart ShoppingCart { set; get; }
}