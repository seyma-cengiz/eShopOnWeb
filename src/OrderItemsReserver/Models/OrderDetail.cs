using System.Collections.Generic;
using Newtonsoft.Json;

namespace OrderItemsReserver.Models;
public class OrderDetail
{
    [JsonProperty("order-id")]
    public int OrderId { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderDetailItem> Items { get; set; }
}

public class OrderDetailItem
{
    [JsonProperty("item-id")]
    public int ItemId { get; set; }
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
