using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public string? PriceUnit { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
