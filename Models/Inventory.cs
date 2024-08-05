using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? StockQuantity { get; set; }

    public string? WarehouseLocation { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Product? Product { get; set; }
}
