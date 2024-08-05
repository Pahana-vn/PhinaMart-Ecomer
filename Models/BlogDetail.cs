using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class BlogDetail
{
    public int Id { get; set; }

    public int? BlogId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Blog? Blog { get; set; }
}
