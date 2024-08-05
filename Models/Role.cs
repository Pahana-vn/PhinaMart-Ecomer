using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? UserId { get; set; }

    public int? PageId { get; set; }

    public bool? AddPermission { get; set; }

    public bool? EditPermission { get; set; }

    public bool? DeletePermission { get; set; }

    public bool? ViewPermission { get; set; }

    public int? Status { get; set; }

    public virtual User? User { get; set; }
}
