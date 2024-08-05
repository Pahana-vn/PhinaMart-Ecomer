namespace PhinaMart.ViewModels
{
    public class WishListVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public bool InStock { get; set; }
    }
}
