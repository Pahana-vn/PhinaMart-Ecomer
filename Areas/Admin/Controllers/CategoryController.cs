using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Models;

namespace PhinaMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]

    public class CategoryController : Controller
    {
        private readonly PhinaMartContext _PhinaContext;
        public CategoryController(PhinaMartContext context)
        {
            _PhinaContext = context;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _PhinaContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]

        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category cate)
        {
            if (ModelState.IsValid)
            {
                cate.Slug = cate.Name.Replace(" ", "-");
                var slug = await _PhinaContext.Categories.FirstOrDefaultAsync(p => p.Slug == cate.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(cate);
                }

                _PhinaContext.Add(cate);
                await _PhinaContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(cate);
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            Category cate = await _PhinaContext.Categories.FindAsync(Id);
            return View(cate);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category cate)
        {
            if (ModelState.IsValid)
            {
                cate.Slug = cate.Name.Replace(" ", "-");

                _PhinaContext.Update(cate);
                await _PhinaContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            Category category = await _PhinaContext.Categories.FindAsync(Id);

            _PhinaContext.Categories.Remove(category);
            await _PhinaContext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã được xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
