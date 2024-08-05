using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Models;
using PhinaMart.Services;
using PhinaMart.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhinaMart.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        private readonly PhinaMartContext _context;
        private readonly WishlistService wishlistService;
        public WishListController(PhinaMartContext context,WishlistService wishlistService)
        {
            _context = context;
            this.wishlistService = wishlistService;

        }
       

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User is not logged in.");
            }

            var userId = int.Parse(userIdClaim);
            var wishListItems = await _context.WishLists
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .Select(w => new WishListVm
                {
                    ProductId = w.ProductId.Value,
                    ProductName = w.Product.Name,
                    ProductImage = w.Product.Image,
                    ProductPrice = w.Product.Price,
                    InStock = w.Product.StockQuantity > 0
                })
                .ToListAsync();

            return View(wishListItems);
        }
        [HttpPost]
        [Route("AddWishList/{id}")]
        public IActionResult AddWishList(int id)
        {
            var result=wishlistService.AddToWishList(id);
            if (result)
            {
                TempData["Message"] = "wishlist add successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["Error"] = "Failed to add wishlist";
                return RedirectToAction("Index", "Product");
              
            }
          
        }
        [HttpPost]
        [Route("AddToWishList/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult AddToWishList(int id )
        {
            var result = wishlistService.AddToWishList(id);
            if (result)
            {
                TempData["Message"] = "wishlist add successfully";
                return RedirectToAction("Detail", "Product", new { id = id });
            }
            else
            {
                TempData["Error"] = "Failed to add wishlist";
                return RedirectToAction("Detail", "Product", new { id = id });
            }
         
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User is not logged in.");
            }

            var userId = int.Parse(userIdClaim);
            var wishListItem = await _context.WishLists.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (wishListItem != null)
            {
                _context.WishLists.Remove(wishListItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
