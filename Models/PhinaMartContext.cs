using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhinaMart.Models;

public partial class PhinaMartContext : DbContext
{
    public PhinaMartContext()
    {
    }

    public PhinaMartContext(DbContextOptions<PhinaMartContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogDetail> BlogDetails { get; set; }

    public virtual DbSet<BlogReply> BlogReplies { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Compare> Compares { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PageWeb> PageWebs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<StarRating> StarRatings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0QD3R2K;Initial Catalog=PhinaMart;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC07AAA03016");

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogComm__3214EC0765548345");

            entity.HasIndex(e => e.BlogId, "IX_BlogComments_Blog_id");

            entity.HasIndex(e => e.UserId, "IX_BlogComments_User_id");

            entity.Property(e => e.BlogId).HasColumnName("Blog_id");
            entity.Property(e => e.CommentText).HasColumnName("Comment_text");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__BlogComme__Blog___5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BlogComme__User___5629CD9C");
        });

        modelBuilder.Entity<BlogDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogDeta__3214EC0712BFEE10");

            entity.HasIndex(e => e.BlogId, "IX_BlogDetails_Blog_id");

            entity.Property(e => e.BlogId).HasColumnName("Blog_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogDetails)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__BlogDetai__Blog___52593CB8");
        });

        modelBuilder.Entity<BlogReply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogRepl__3214EC0793A752A7");

            entity.HasIndex(e => e.BlogCommentsId, "IX_BlogReplies_BlogComments_id");

            entity.HasIndex(e => e.UserId, "IX_BlogReplies_User_id");

            entity.Property(e => e.BlogCommentsId).HasColumnName("BlogComments_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.ReplyText).HasColumnName("Reply_text");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.BlogComments).WithMany(p => p.BlogReplies)
                .HasForeignKey(d => d.BlogCommentsId)
                .HasConstraintName("FK__BlogRepli__BlogC__59063A47");

            entity.HasOne(d => d.User).WithMany(p => p.BlogReplies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BlogRepli__User___59FA5E80");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC07C7265AD2");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("Company_name");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(255)
                .HasColumnName("Contact_person");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Slug).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07CA04BFCB");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC079DA26B2A");

            entity.HasIndex(e => e.ProductId, "IX_Comments_Product_id");

            entity.HasIndex(e => e.UserId, "IX_Comments_User_id");

            entity.Property(e => e.CommentText).HasColumnName("Comment_text");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Comments__Produc__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__User_i__5CD6CB2B");
        });

        modelBuilder.Entity<Compare>(entity =>
        {
            entity.ToTable("Compare");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Compares)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compare_Products");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Compares)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compare_Users");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC07E3E8F761");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Discount_percentage");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_date");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC0758897F73");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.ProductId, "IX_Inventory_Product_id");

            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime")
                .HasColumnName("Last_updated");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.StockQuantity).HasColumnName("Stock_quantity");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(255)
                .HasColumnName("Warehouse_location");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Inventory__Produ__71D1E811");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC076E6BFD4E");

            entity.HasIndex(e => e.UserId, "IX_Notifications_User_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.IsRead).HasColumnName("Is_read");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__User___74AE54BC");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC074A6F40F1");

            entity.HasIndex(e => e.UserId, "IX_Orders_User_id");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("Delivery_date");
            entity.Property(e => e.HowToPay)
                .HasMaxLength(255)
                .HasColumnName("How_to_pay");
            entity.Property(e => e.HowToTransport)
                .HasMaxLength(255)
                .HasColumnName("How_to_transport");
            entity.Property(e => e.IdCustomer).HasColumnName("Id_customer");
            entity.Property(e => e.IdStaff).HasColumnName("Id_staff");
            entity.Property(e => e.OrderCode)
                .HasMaxLength(255)
                .HasColumnName("Order_code");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order_date");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_amount");
            entity.Property(e => e.TransportFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Transport_fee");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("User_name");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__User_id__412EB0B6");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order_de__3214EC07AA2AF4B1");

            entity.ToTable("Order_details");

            entity.HasIndex(e => e.OrderId, "IX_Order_details_Order_id");

            entity.HasIndex(e => e.ProductId, "IX_Order_details_Product_id");

            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceUnit)
                .HasMaxLength(255)
                .HasColumnName("Price_unit");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_det__Order__440B1D61");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Order_det__Produ__44FF419A");
        });

        modelBuilder.Entity<PageWeb>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PageWeb");

            entity.Property(e => e.NamePage)
                .HasMaxLength(255)
                .HasColumnName("Name_page");
            entity.Property(e => e.Url).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07229037B8");

            entity.HasIndex(e => e.BrandId, "IX_Products_Brand_id");

            entity.HasIndex(e => e.CategoryId, "IX_Products_Category_id");

            entity.Property(e => e.BrandId).HasColumnName("Brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.Color).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DescriptionUnit)
                .HasMaxLength(255)
                .HasColumnName("Description_unit");
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasMaxLength(255);
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.StockQuantity).HasColumnName("Stock_Quantity");
            entity.Property(e => e.TypeCode)
                .HasMaxLength(50)
                .HasColumnName("Type_code");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.ViewLuot).HasColumnName("View_luot");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Products__Brand___3C69FB99");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__3B75D760");
        });

        modelBuilder.Entity<ProductDiscount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductD__3214EC070BC147F0");

            entity.HasIndex(e => e.DiscountId, "IX_ProductDiscounts_Discount_id");

            entity.HasIndex(e => e.ProductId, "IX_ProductDiscounts_Product_id");

            entity.Property(e => e.DiscountId).HasColumnName("Discount_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Discount).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__ProductDi__Disco__6EF57B66");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductDi__Produ__6E01572D");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Products");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Users");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Replies__3214EC07C98D319F");

            entity.HasIndex(e => e.CommentId, "IX_Replies_Comment_id");

            entity.HasIndex(e => e.UserId, "IX_Replies_User_id");

            entity.Property(e => e.CommentId).HasColumnName("Comment_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.ReplyText).HasColumnName("Reply_text");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__Replies__Comment__60A75C0F");

            entity.HasOne(d => d.User).WithMany(p => p.Replies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Replies__User_id__619B8048");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07B804989E");

            entity.HasIndex(e => e.ProductId, "IX_Reviews_Product_id");

            entity.HasIndex(e => e.UserId, "IX_Reviews_User_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Reviews__Product__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__User_id__693CA210");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0753F98AB0");

            entity.ToTable("Role");

            entity.HasIndex(e => e.UserId, "IX_Role_User_id");

            entity.Property(e => e.AddPermission).HasColumnName("Add_permission");
            entity.Property(e => e.DeletePermission).HasColumnName("Delete_permission");
            entity.Property(e => e.EditPermission).HasColumnName("Edit_permission");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PageId).HasColumnName("Page_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.ViewPermission).HasColumnName("View_permission");

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Role__User_id__48CFD27E");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("User_name");
        });

        modelBuilder.Entity<StarRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StarRati__3214EC070966D6FA");

            entity.HasIndex(e => e.ProductId, "IX_StarRatings_Product_id");

            entity.HasIndex(e => e.UserId, "IX_StarRatings_User_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.StarRatings)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__StarRatin__Produ__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.StarRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__StarRatin__User___6477ECF3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07D3FE3EE8");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(255)
                .HasColumnName("Random_key");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WishList__3214EC074C02B87F");

            entity.ToTable("WishList");

            entity.HasIndex(e => e.ProductId, "IX_WishList_Product_id");

            entity.HasIndex(e => e.UserId, "IX_WishList_User_id");

            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.SelectDate)
                .HasColumnType("datetime")
                .HasColumnName("Select_date");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__WishList__Produc__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__WishList__User_i__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
