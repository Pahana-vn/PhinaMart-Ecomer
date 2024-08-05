using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Models;

namespace PhinaMart.Repository.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly PhinaMartContext _PhinaContext;

        public CategoriesViewComponent(PhinaMartContext context)
        {
            _PhinaContext = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await _PhinaContext.Categories.ToListAsync());
    }
}
