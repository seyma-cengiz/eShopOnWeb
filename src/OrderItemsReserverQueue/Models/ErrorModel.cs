using Newtonsoft.Json;

namespace OrderItemsReserverQueue.Models;
public class ErrorModel
{
    [JsonProperty("order-item")]
    public string OrderItem { get; set; }
    [JsonProperty("error-message")]
    public string ErrorMessage { get; set; }
}
