using Newtonsoft.Json;

namespace BuzzHouse.Model.Models;

public class User
{
    [JsonProperty("id")]
    public Guid Id { set; get; }
    [JsonProperty("email")]
    public string Email { set; get; }
    [JsonProperty("firstname")]
    public string FirstName { set; get; }
    [JsonProperty("lastName")]
    public string LastName { set; get; }
    [JsonProperty("shippingAddress")]
    public string ShippingAddress { set; get; }
}