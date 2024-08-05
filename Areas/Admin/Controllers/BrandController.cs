using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Models;

namespace PhinaMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Brand")]

    public class BrandController : Controller
    {
        private readonly PhinaMartContext _PhinaMart;
        public BrandController(PhinaMartContext context)
        {
            _PhinaMart = context;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _PhinaMart.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]

        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand br)
        {
            if (ModelState.IsValid)
            {
                br.Slug = br.CompanyName.Replace(" ", "-");
                var slug = await _PhinaMart.Brands.FirstOrDefaultAsync(p => p.Slug == br.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(br);
                }

                _PhinaMart.Add(br);
                await _PhinaMart.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
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

        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            Brand brand = await _PhinaMart.Brands.FindAsync(Id);
            return View(brand);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.CompanyName.Replace(" ", "-");
                _PhinaMart.Update(brand);
                await _PhinaMart.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công";
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
            Brand br = await _PhinaMart.Brands.FindAsync(Id);
            _PhinaMart.Brands.Remove(br);
            await _PhinaMart.SaveChangesAsync();
            TempData["success"] = "Thương hiệu đã được xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
