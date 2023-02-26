using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Infrastructure.Data;
public class OrderRequestItem
{
    public int Qty { get; set; }
    public int ItemId { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public string? PictureUrl { get; set; }
}

public class OrderRequest
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get => "OrderId_" + OrderId.ToString(); }
    public int OrderId { get; set; }
    public OrderRequestItem[] Items { get; set; }
    public decimal TotalPrice { get; set; }
    public Address Address { get; set; }
}
