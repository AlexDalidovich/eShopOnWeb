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
using Microsoft.Azure.Cosmos;
using System.Net;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Configuration;

namespace FunctionApp
{

    public class OrderItemsReserver
    {
        const string ContainerName = "reservations";
        private readonly IConfiguration _configuration;

        public OrderItemsReserver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName(nameof(OrderItemsReserver))]
        public async Task<IActionResult> Run([HttpTrigger("post")] string body,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var order = JsonSerializer.Deserialize<OrderRequest>(body);
            var o = JsonSerializer.Serialize(order);
            log.LogInformation(o);
            
            BlobServiceClient client = new(_configuration.GetConnectionString("BlobConnectionString"));
            BlobContainerClient container  = client.GetBlobContainerClient(ContainerName);
            await container.CreateIfNotExistsAsync();
            
            var blobClient = container.GetBlobClient($"{order.Id}-{Guid.NewGuid()}.json");
            using MemoryStream stream = new(Encoding.ASCII.GetBytes(o));
            var resp = await blobClient.UploadAsync(stream);
            log.LogInformation(JsonSerializer.Serialize(resp));

            return new OkObjectResult("Ok");
        }
    }
}
