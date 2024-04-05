namespace BuzzHouse.Model.Models;

public class ShoppingCart
{
    public Guid Id { set; get; }
    public int UserId { set; get; }
    public List<CartItem> CartItems { set; get; }
}