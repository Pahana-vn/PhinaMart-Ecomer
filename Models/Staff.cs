using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Staff
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Status { get; set; }
}
