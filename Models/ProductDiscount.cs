using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class ProductDiscount
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual Product? Product { get; set; }
}
