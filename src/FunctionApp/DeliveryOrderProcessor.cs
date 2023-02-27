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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp;

public class DeliveryOrderProcessor
{
    const string ContainerName = "reservations";
    private readonly IConfiguration _configuration;

    public DeliveryOrderProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [FunctionName(nameof(DeliveryOrderProcessor))]
    public async Task<IActionResult> Run([HttpTrigger("post")] HttpRequest req, ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        log.LogInformation("C# HTTP trigger function processed a request.");
        var order = JsonSerializer.Deserialize<OrderRequest>(requestBody);
        var o = JsonSerializer.Serialize(order);
        log.LogInformation(o);

        CosmosClient client = new(_configuration.GetConnectionString("CosmosConnectionString"));
        Database database = await client.CreateDatabaseIfNotExistsAsync("eShopOnWeb");
        Container container = await database.CreateContainerIfNotExistsAsync("OrderDetails", "/id");
        var resp = await container.CreateItemAsync(order);
        log.LogInformation(JsonSerializer.Serialize(resp));

        return new OkObjectResult("Ok");
    }
}
