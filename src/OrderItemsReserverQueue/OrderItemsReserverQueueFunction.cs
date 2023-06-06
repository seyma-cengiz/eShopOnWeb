using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderItemsReserverQueue.Models;

namespace OrderItemsReserverQueue;

public class OrderItemsReserverQueueFunction
{
    private static HttpClient httpClient = new HttpClient();

    [FunctionName("OrderItemsReserverQueueFunction")]
    public async static Task Run(
        [ServiceBusTrigger(
            "order-items-reserver-queue",
            Connection = "ServiceBusConnection")]string orderQueueItem,
        ILogger log)
    {
        var connectionString = Environment.GetEnvironmentVariable("BlobStorageConnection");
        log.LogInformation("function triggered");

        if (string.IsNullOrWhiteSpace(orderQueueItem))
        {
            log.LogWarning("Order detail is null");
            return;
        }

        try
        {
            var order = JsonConvert.DeserializeObject<OrderDetailModel>(orderQueueItem);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("orders");

            await containerClient.CreateIfNotExistsAsync();

            var response = await containerClient.UploadBlobAsync($"order-{order.OrderId}.json", BinaryData.FromString(orderQueueItem));
        }
        catch (Exception ex)
        {
            log.LogWarning($"Error occured!\n{ex.Message}");
            var error = JsonConvert.SerializeObject(new ErrorModel()
            {
                OrderItem = orderQueueItem,
                ErrorMessage = ex.Message
            });
            var logicAppUri = Environment.GetEnvironmentVariable("LogicAppUri");
            await httpClient.PostAsync(logicAppUri, new StringContent(error, Encoding.UTF8, "application/json"));
        }
    }
}
