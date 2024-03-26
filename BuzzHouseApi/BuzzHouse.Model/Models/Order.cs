using System.Runtime.InteropServices.JavaScript;

namespace BuzzHouse.Model.Models;

public class Order
{
    public int id { set; get; }
    public DateTime createdDate { set; get; }
    public DateTime deliveryDate { set; get; }
    public int userId { set; get; }
    public string shippingAddress { set; get; }
    public int orderStatus { set; get; }
    public ShoppingCart shoppingCart { set; get; }
}