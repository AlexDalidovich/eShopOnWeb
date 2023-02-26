using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.Azure.Cosmos;
using System.Net;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace FunctionApp
{

    public class OrderItemsReserver
    {
        [FunctionName(nameof(OrderItemsReserver))]
        public static async Task<IActionResult> Run([HttpTrigger("post")] string body,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var order = JsonSerializer.Deserialize<OrderRequest>(body);
            var o = JsonSerializer.Serialize(order);
            log.LogInformation(o);

            BlobServiceClient client = new("DefaultEndpointsProtocol=https;AccountName=csb10032001f4ab297a;AccountKey=VdcCqgM9cK1XNX8Gv28iPec6vELRffNGILEipI/cmWmpTyWfIgVg6DYN0JsfMuwI4IjFBGt3K1SD+AStdc1ylQ==;EndpointSuffix=core.windows.net");
            BlobContainerClient container = await client.CreateBlobContainerAsync("reservations");
            var blobClient = container.GetBlobClient($"{order.Id}-{Guid.NewGuid()}.json");
            using MemoryStream stream = new(Encoding.ASCII.GetBytes(o));
            var resp = await blobClient.UploadAsync(stream);
            log.LogInformation(JsonSerializer.Serialize(resp));

            return new OkObjectResult("Ok");
        }
    }
}
