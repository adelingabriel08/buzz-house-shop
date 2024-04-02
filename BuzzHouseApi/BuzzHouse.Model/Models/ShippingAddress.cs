namespace BuzzHouse.Model.Models;

public class ShippingAddress
{
    public string Street { set; get; }
    public string Number { set; get; }
    public string? ApartmentNumber { set; get; }
    public string? Floor { set; get; }
    public string? AdditionalDetails { set; get; }
    public string City { set; get; }
    public string PostalCode { set; get; }
    public string Country { set; get; }
}