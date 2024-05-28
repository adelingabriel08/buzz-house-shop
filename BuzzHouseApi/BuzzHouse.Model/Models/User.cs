using Newtonsoft.Json;

namespace BuzzHouse.Model.Models;

public class User
{
    [JsonProperty("id")]
    public string Id { set; get; }
    
    [JsonProperty("firstname")]
    public string FirstName { set; get; }
    [JsonProperty("lastName")]
    public string LastName { set; get; }
    [JsonProperty("shippingAddress")]
    public string ShippingAddress { set; get; }
}