namespace PhinaMart.Services
{
    public interface StaticalService
    {
        public int ShowOrder();
        public decimal TotalOrderEarnByMonth();
        public int ToTalProduct();
        public int TotalCategory();
        public dynamic GetCountOrder( int datetime);
        public dynamic GetRevenueByMonth();
    }
}
