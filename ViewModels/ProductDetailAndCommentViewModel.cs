namespace PhinaMart.ViewModels
{
    public class ProductDetailAndCommentViewModel
    {
        public DetailProductVm ProductDetail { get; set; }
        public CreateComment NewComment { get; set; }

        public CreateRating Score { get; set; }

        public DisplayRating DisplayRating { get; set; }
        public int Quantity { get; set; }
    }
    
}
