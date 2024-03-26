namespace BuzzHouse.Model.Models;

public class Product
{
    public int id { set; get; }
    public int type { set; get; }
    public int name { set; get; }
    public string description { set; get; }
    public double price { set; get; }
    public int stock { set; get; }
    public string imageUrl { set; get; }
    public bool custom { get; set; }
}