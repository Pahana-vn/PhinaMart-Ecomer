using PhinaMart.Repository.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhinaMart.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public int? BrandId { get; set; }

    public decimal? Discount { get; set; }

    public int? ViewLuot { get; set; }

    public int? StockQuantity { get; set; }

    public int? CategoryId { get; set; }

    public string? Image { get; set; }

    public string? TypeCode { get; set; }

    public string? DescriptionUnit { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Quantity { get; set; }

    public virtual Brand? Brand { get; set; }

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }

    public virtual Category? Category { get; set; }


    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Compare> Compares { get; set; } = new List<Compare>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<StarRating> StarRatings { get; set; } = new List<StarRating>();

    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}
