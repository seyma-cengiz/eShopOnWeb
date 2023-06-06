using System.Collections.Generic;
using Newtonsoft.Json;

namespace OrderItemsReserverQueue.Models;
public class OrderDetailModel
{
    [JsonProperty("order-id")]
    public int OrderId { get; set; }
    [JsonProperty("items")]
    public IReadOnlyCollection<OrderDetailItemModel> Items { get; set; }
}

public class OrderDetailItemModel
{
    [JsonProperty("item-id")]
    public int ItemId { get; set; }
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
