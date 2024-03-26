namespace BuzzHouse.Model.Models;

public class CartItem
{
    public Product product { set; get; }
    public int quantity { set; get; }
    public int productSize { set; get; }
    public string customDetails { set; get; }
    public string customImg { set; get; }
    public double price { set; get; }
}