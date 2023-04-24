using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Models;

public class OrderDetailModel
{
    [JsonProperty("shipping-address")]
    public string? ShippingAddress { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderDetailItemModel>? Items { get; set; }
    [JsonProperty("total-price")]
    public decimal TotalPrice { get; set; }
}

public class OrderDetailItemModel
{
    [JsonProperty("item-id")]
    public int Id { get; set; }
    [JsonProperty("catalog-item-id")]
    public int CatalogItemId { get; set; }
    [JsonProperty("product-name")]
    public string? ProductName { get; set; }
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

//public class OrderDetailModel
//{
//    [JsonProperty("order-id")]
//    public int OrderId { get; set; }
//    [JsonProperty("items")]
//    public IReadOnlyCollection<OrderDetailItemModel>? Items { get; set; }
//}

//public class OrderDetailItemModel
//{
//    [JsonProperty("item-id")]
//    public int ItemId { get; set; }
//    [JsonProperty("quantity")]
//    public int Quantity { get; set; }
//}
