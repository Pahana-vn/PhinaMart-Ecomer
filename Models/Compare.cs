using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Compare
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public int IdUser { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
