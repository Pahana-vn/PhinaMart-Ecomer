using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? IdCustomer { get; set; }

    public int? IdStaff { get; set; }

    public string? OrderCode { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? UserName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? HowToPay { get; set; }

    public string? HowToTransport { get; set; }

    public decimal? TransportFee { get; set; }

    public int? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
