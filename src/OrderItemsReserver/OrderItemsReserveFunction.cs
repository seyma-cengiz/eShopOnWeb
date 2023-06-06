using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderItemsReserver.Models;

namespace OrderItemsReserver;

public static class OrderItemsReserveFunction
{
    [FunctionName("OrderItemsReserveFunction")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/reserve")] OrderDetail orderDetail,
        [Blob("orders/order-{rand-guid}.json",
              FileAccess.Write),
        StorageAccount("OrderFunctionBlobConnection")] out string orderBlob,
        ILogger log)
    {
        log.LogInformation("Order function processed a request.");
        orderBlob = default;

        if (orderDetail == null)
        {
            return new BadRequestObjectResult("Order detail is null");
        }

        orderBlob = JsonConvert.SerializeObject(orderDetail);
        return new AcceptedResult();
    }
}
