namespace BuzzHouse.Model.Models;

public class Product
{
    public int Id { set; get; }
    public int Type { set; get; }
    public int Name { set; get; }
    public string Description { set; get; }
    public double Price { set; get; }
    public int Stock { set; get; }
    public string? ImageUrl { set; get; }
    public bool Custom { get; set; }
}