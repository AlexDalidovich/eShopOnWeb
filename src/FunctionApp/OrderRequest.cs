using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FunctionApp;
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

public class Address // ValueObject
{
    public string Street { get;   set; }

    public string City { get;   set; }

    public string State { get;   set; }

    public string Country { get;   set; }

    public string ZipCode { get;   set; }
}
