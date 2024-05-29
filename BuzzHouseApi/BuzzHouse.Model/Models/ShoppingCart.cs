namespace BuzzHouse.Model.Models;

public class ShoppingCart
{
    public string Id { set; get; }
    public string UserId { set; get; }
    public List<CartItem> CartItems { set; get; }
}