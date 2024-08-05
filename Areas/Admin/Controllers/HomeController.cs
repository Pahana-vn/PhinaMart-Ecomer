using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhinaMart.Models;
using PhinaMart.Services;
using PhinaMart.ViewModels;

namespace PhinaMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Home")]

    public class HomeController : Controller
    {
        private StaticalService staticalService;
        private PhinaMartContext context;
        public HomeController(StaticalService staticalService,PhinaMartContext context)
        {
            this.staticalService = staticalService; 
            this.context = context;
        }
        [Route("Index")]
        public IActionResult Index()
        {
           
            var topProducts = context.OrderDetails
        .GroupBy(od => od.ProductId)
        .Select(g => new
        {
            ProductId = g.Key,
            TotalSold = g.Sum(od => od.Quantity)
        })
        .OrderByDescending(x => x.TotalSold)
        .Take(5)
        .Join(context.Products,
              g => g.ProductId,
              p => p.Id,
              (g, p) => new ProductVm
              {
                  Id = p.Id,
                  Name = p.Name,
                  Price = p.Price,
                  Image = p.Image ?? "",
                  DescriptionShort = p.DescriptionUnit ?? string.Empty,
                  NameCategory = p.Category.Name
              })
        .ToList();
            ViewBag.Order = staticalService.ShowOrder();
            ViewBag.Earning=staticalService.TotalOrderEarnByMonth();
            ViewBag.Product=staticalService.ToTalProduct();
            ViewBag.Category=staticalService.TotalCategory();
            ViewBag.TopProducts=topProducts;
         
            int currentMonth = DateTime.Now.Month;
            ViewBag.CurrentMonth = currentMonth;
            return View();
        }
        [HttpGet("GetVenueByMonth")]
        public IActionResult GetVenueByMonth()
        {
            return Ok(staticalService.GetRevenueByMonth());
        }
        [HttpGet("MonthlyOrderChart/{month}")]
        public IActionResult GetMonthlyOrderCount(int month)
        {
            var orderCounts = staticalService.GetCountOrder(month);
            return Ok(orderCounts); // Return orderCounts as JSON
        }


    }
}
