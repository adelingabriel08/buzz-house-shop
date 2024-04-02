using System.Runtime.InteropServices.JavaScript;

namespace BuzzHouse.Model.Models;

public class Order
{
    public int Id { set; get; }
    public DateTime CreatedDate { set; get; }
    public DateTime DeliveryDate { set; get; }
    public int UserId { set; get; }
    public ShippingAddress ShippingAddress { set; get; }
    public int OrderStatus { set; get; }
    public ShoppingCart ShoppingCart { set; get; }
}