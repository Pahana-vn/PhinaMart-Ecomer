namespace PhinaMart.ViewModels
{
    public class ProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string DescriptionShort { get; set; }
        public string NameCategory { get; set; }
        public int Quantity { get; set; }
        public double AverageRating { get; set; }
        public double TotalRatings { get; set; }

    }

    public class DetailProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string DescriptionShort { get; set; }
        public string NameCategory { get; set; }
        public string Description { get; set; }
        public int StarRating { get; set; }
        public int StockQuantity { get; set; }

        public int Quantity { get; set; }
        public List<PersonalRatingVm> PersonalRatings { get; set; }
    }
    public class PersonalRatingVm
    {
        public int UserId { get; set; }
        public double AverageRating { get; set; }
    }
}