using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Models;


public class OrderProcessModel
{
    [JsonProperty("shipping-address")]
    public string? ShippingAddress { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderProcessItemModel>? Items { get; set; }
    [JsonProperty("total-price")]
    public decimal TotalPrice { get; set; }
}

public class OrderProcessItemModel
{
    [JsonProperty("item-id")]
    public int Id { get; set; }
    [JsonProperty("unit-price")]
    public decimal UnitPrice { get; set; }
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}

//public class AddressModel
//{
//    public string Street { get; private set; }

//    public string City { get; private set; }

//    public string State { get; private set; }

//    public string Country { get; private set; }

//    public string ZipCode { get; private set; }

//#pragma warning disable CS8618 // Required by Entity Framework
//    private AddressModel() { }

//    public AddressModel(string street, string city, string state, string country, string zipcode)
//    {
//        Street = street;
//        City = city;
//        State = state;
//        Country = country;
//        ZipCode = zipcode;
//    }
//}
