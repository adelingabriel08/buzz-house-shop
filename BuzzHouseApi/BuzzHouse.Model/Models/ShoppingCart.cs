namespace BuzzHouse.Model.Models;

public class ShoppingCart
{
    public int id { set; get; }
    public int userId { set; get; }
    public List<CartItem> cartItems { set; get; }
}