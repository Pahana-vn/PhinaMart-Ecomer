using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class BlogReply
{
    public int Id { get; set; }

    public int? BlogCommentsId { get; set; }

    public int? UserId { get; set; }

    public string? ReplyText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BlogComment? BlogComments { get; set; }

    public virtual User? User { get; set; }
}
