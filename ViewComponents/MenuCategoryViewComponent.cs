using Microsoft.AspNetCore.Mvc;
using PhinaMart.Models;
using PhinaMart.ViewModels;

namespace PhinaMart.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly PhinaMartContext db;

        public MenuCategoryViewComponent(PhinaMartContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(lo => new MenuCategoryVM
            {
                Id = lo.Id,
                Name = lo.Name,
                Quantity = lo.Products.Count
            }).OrderBy(p => p.Name);
            return View(data);
        }
    }
}
