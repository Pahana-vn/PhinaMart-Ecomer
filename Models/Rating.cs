using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Rating
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdProduct { get; set; }

    public DateTime CreateDate { get; set; }

    public int Score { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
