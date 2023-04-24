using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OrderProcess.Models;
public class OrderDetail
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("shipping-address")]
    public string ShippingAddress { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderDetailItem> Items { get; set; }
    [JsonProperty("total-price")]
    public decimal TotalPrice { get; set; }
}

public class OrderDetailItem
{
    [JsonProperty("item-id")]
    public int Id { get; set; }
    [JsonProperty("catalog-item-id")]
    public int CatalogItemId { get; set; }
    [JsonProperty("product-name")]
    public string ProductName { get; set; }
    [JsonProperty("unit-price")]
    public decimal UnitPrice { get; set; }
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
