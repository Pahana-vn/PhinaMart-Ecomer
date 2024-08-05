using System;
using System.Collections.Generic;

namespace PhinaMart.Models;

public partial class BlogComment
{
    public int Id { get; set; }

    public int? BlogId { get; set; }

    public int? UserId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual ICollection<BlogReply> BlogReplies { get; set; } = new List<BlogReply>();

    public virtual User? User { get; set; }
}
