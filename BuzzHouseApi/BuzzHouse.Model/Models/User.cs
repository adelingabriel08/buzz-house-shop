namespace BuzzHouse.Model.Models;

public class User
{
    public Guid Id { set; get; }
    public string Email { set; get; }
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string ShippingAddress { set; get; }
}