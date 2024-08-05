using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class Reply
{
    public int Id { get; set; }

    public int? CommentId { get; set; }

    public int? UserId { get; set; }

    public string? ReplyText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual User? User { get; set; }
}
