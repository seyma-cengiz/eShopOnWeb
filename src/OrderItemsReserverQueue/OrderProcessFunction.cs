using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OrderItemsReserverQueue.Models;

namespace OrderItemsReserverQueue;

public static class OrderProcessFunction
{
    [FunctionName("OrderProcessFunction")]
    public static IActionResult Run(
     [HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/process")] OrderProcessModel order,
     [CosmosDB(databaseName: "EShopOnWeb", containerName: "Orders", Connection = "CosmosDBConnection")] out OrderProcessModel orderDocument,
     ILogger log)
    {
        log.LogInformation("Order successfully forwarded to the warehouse");

        orderDocument = default;
        if (order == null)
        {
            return new BadRequestObjectResult("Order detail is null");
        }
        order.Id = Guid.NewGuid();
        orderDocument = order;
        return new AcceptedResult();
    }
}
