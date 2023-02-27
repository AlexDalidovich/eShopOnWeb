using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp;

public class DeliveryOrderProcessor
{
    [FunctionName(nameof(DeliveryOrderProcessor))]
    public static async Task<IActionResult> Run([HttpTrigger("post")] HttpRequest req, ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        log.LogInformation("C# HTTP trigger function processed a request.");
        var order = JsonSerializer.Deserialize<OrderRequest>(requestBody);
        var o = JsonSerializer.Serialize(order);
        log.LogInformation(o);

        CosmosClient client = new("AccountEndpoint=https://e-shop-on-web.documents.azure.com:443/;AccountKey=jdAsCPz27O6rTW9S62yD5RP188aSm3GwQO5mDrLWTMyJtz2TYI4kxfaY5CSA7ST0vQRH4sNvplMDACDb58JhPw==;");
        Database database = await client.CreateDatabaseIfNotExistsAsync("eShopOnWeb");
        Container container = await database.CreateContainerIfNotExistsAsync("OrderDetails", "/id");
        var resp = await container.CreateItemAsync(order);
        log.LogInformation(JsonSerializer.Serialize(resp));

        return new OkObjectResult("Ok");
    }
}
