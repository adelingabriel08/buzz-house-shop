namespace BuzzHouse.Model.Models;

public class CartItem
{
    public Product Product { set; get; }
    public int Quantity { set; get; }
    public int ProductSize { set; get; }
    public string? CustomDetails { set; get; }
    public string? CustomImg { set; get; }
    public double Price { set; get; }
}