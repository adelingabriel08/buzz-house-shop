namespace BuzzHouse.Model.Models;

public class ShoppingCart
{
    public int Id { set; get; }
    public int UserId { set; get; }
    public List<CartItem> CartItems { set; get; }
}