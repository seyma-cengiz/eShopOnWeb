using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OrderProcess.Models;
public class OrderProcessModel
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("shipping-address")]
    public string ShippingAddress { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderProcessItemModel> Items { get; set; }
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
