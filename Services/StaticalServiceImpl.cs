using PhinaMart.Models;

namespace PhinaMart.Services
{
    public class StaticalServiceImpl : StaticalService
    {
        private readonly PhinaMartContext _context;
        public StaticalServiceImpl(PhinaMartContext context)
        {
            _context = context;
        }

        public dynamic GetCountOrder( int datetime)
        {
            return _context.Orders.Where(p => p.OrderDate.HasValue && p.OrderDate.Value.Month == datetime).GroupBy(o => o.OrderDate).Select(g => new
            {
                OrderDate = g.Key,
                OrderCount = g.Count()
            }).OrderBy(x => x.OrderDate)
          .ToList();
        }

        public dynamic GetRevenueByMonth()
        {
           return _context.Orders.Where(p=>p.OrderDate.HasValue).GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month }).Select(g => new
           {
               Year = g.Key.Year,
               Month = g.Key.Month,
               Revenue = g.Sum(o => o.TotalAmount)
           }).OrderBy(x => x.Year)
        .ThenBy(x => x.Month)
        .ToList();
        
    }

        public int ShowOrder()
        {
            return _context.Orders.Count();
        }

        public int TotalCategory()
        {
          return _context.Categories.Count();
        }

        public decimal TotalOrderEarnByMonth()
        {
            var CurrentMonth = DateTime.Now.Month;
            var CurrentYear=DateTime.Now.Year;

            return _context.Orders
                .Where(d => d.OrderDate.HasValue && d.OrderDate.Value.Month == CurrentMonth && d.OrderDate.Value.Year ==CurrentYear)
                .Sum(d => d.TotalAmount ?? 0);
        }

        public int ToTalProduct()
        {
            return _context.Products.Count();
        }
    }
}
