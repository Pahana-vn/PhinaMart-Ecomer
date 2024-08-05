using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Models;
using PhinaMart.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using PhinaMart.Services;
using PhinaMart.Helpers;
using System.Xml.Linq;
using System.Linq;


namespace PhinaMart.Controllers
{
    public class ProductController : Controller
    {
        private readonly PhinaMartContext db;
        private readonly CommentService commentService;
        public ProductController(PhinaMartContext context, CommentService commentService)
        {
            db = context;
            this.commentService = commentService;
        }

        [Route("FindFilter")]
        public IActionResult FindFilter(decimal max, decimal min)
        {
            var result = db.Products.Where(d => d.Price >= min && d.Price <= max).Select(p => new ProductVm
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image ?? "",
                DescriptionShort = p.DescriptionUnit ?? string.Empty,
                NameCategory = p.Category.Name
            }).ToList();
            return View("Index", result);
        }
        public IActionResult Index(int? category)
        {
            var products = db.Products.AsQueryable();

            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }

            var result = products.Select(p => new ProductVm
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image ?? "",
                DescriptionShort = p.DescriptionUnit ?? string.Empty,

                NameCategory = p.Category.Name,
                Quantity = p.Quantity ?? 0,
                AverageRating = db.Ratings.Where(r => r.IdProduct == p.Id).Average(r => (double?)r.Score) ?? 0,
                TotalRatings = db.Ratings.Count(r => r.IdProduct == p.Id)

            });


            return View(result);
        }

        public IActionResult Search(string query)
        {
            var products = db.Products.AsQueryable();

            if (query != null)
            {
                products = products.Where(p => p.Name.Contains(query));
            }

            var result = products.Select(p => new ProductVm
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image ?? "",
                DescriptionShort = p.DescriptionUnit ?? string.Empty,
                NameCategory = p.Category.Name
            });

            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.Id == id);
            if (data == null)
            {
                TempData["error"] = $"{id}";
                return Redirect("/404");
            }
            var totalRatings = db.Ratings.Count(r => r.IdProduct == id);
            var averageRating = totalRatings > 0 ? db.Ratings.Where(r => r.IdProduct == id).Average(r => r.Score) : 0;
            var averageStars = averageRating / 20.0;
            var comments = db.Comments.Where(d => d.ProductId == id).Select(d => new
            {
                Id = d.Id,
                User = d.User.Username,
                CreateDate = d.CreatedAt,
                ContenText = d.CommentText,
                PersonalRating = db.Ratings
                .Where(r => r.IdProduct == id && r.IdUser == d.UserId)
                .Select(r => r.Score)
                .FirstOrDefault()
            }).OrderByDescending(d => d.Id).ToList();

            ViewBag.Comments = comments;
            ViewBag.AverageRating = averageRating;
            ViewBag.countRating = totalRatings;

            var starCounts = new int[5];
            if (totalRatings > 0)
            {
                var groupedRatings = db.Ratings
                    .Where(r => r.IdProduct == id)
                    .GroupBy(r => r.Score / 20)
                    .Select(g => new { Stars = g.Key, Count = g.Count() })
                    .ToList();

                foreach (var group in groupedRatings)
                {
                    if (group.Stars < 1)
                    {
                        starCounts[0] = group.Count;
                    }
                    else if (group.Stars >= 5)
                    {
                        starCounts[4] = group.Count;
                    }
                    else
                    {
                        starCounts[group.Stars - 1] = group.Count;
                    }
                }
            }

            var starPercentages = new double[5];
            for (int i = 0; i < starCounts.Length; i++)
            {
                starPercentages[i] = (starCounts[i] * 100.0) / totalRatings;
            }

            var averageRatingPersonal = db.Ratings
                .Where(r => r.IdProduct == id)
                .GroupBy(r => r.IdUser)
                .Select(g => new PersonalRatingVm
                {
                    UserId = g.Key,
                    AverageRating = g.Average(r => r.Score)
                })
                .ToList();

            var viewModel = new ProductDetailAndCommentViewModel
            {
                ProductDetail = new DetailProductVm
                {
                    Id = data.Id,
                    Name = data.Name,
                    Price = data.Price,
                    Description = data.Description ?? string.Empty,
                    Image = data.Image ?? string.Empty,
                    DescriptionShort = data.DescriptionUnit ?? string.Empty,
                    NameCategory = data.Category.Name,
                    StockQuantity = 10, // Placeholder
                    StarRating = 5, // Placeholder
                    Quantity = data.Quantity ?? 0,
                    PersonalRatings = averageRatingPersonal
                },
                NewComment = new CreateComment(),
                Score = new CreateRating(),
                DisplayRating = new DisplayRating
                {
                    TotalRatings = totalRatings,
                    AverageStars = averageStars,
                    AverageRating = averageRating,
                    StarPercentages = starPercentages
                }
            };

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            ViewBag.Email = email;

            return View(viewModel);
        }



        [HttpPost]
        [Route("CreateComp/{id}")]
        public IActionResult CreateComp(int id)
        {

            var idUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idUser == null)
            {

                return RedirectToAction("Login", "User");
            }
            else
            {
                var total = db.Compares.Where(d => d.IdUser == int.Parse(idUser) && d.IdProduct == id).Count();
                if (total >= 4)
                {
                    TempData["Exist"] = "There are 4 products in compare";
                    return RedirectToAction("Index", "Product");
                }
                if (db.Compares.Any(d => d.IdUser == int.Parse(idUser) && d.IdProduct == id))
                {
                    TempData["Exist"] = "this Product is already exists in Compare";
                    return RedirectToAction("Index", "Product");
                }
                var result = commentService.CreateCompare(id);
                if (result)
                {
                    TempData["Success"] = "Compare Created Successfully";
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    TempData["Error"] = "Failed to create comment";
                    return RedirectToAction("Index", "Product");
                }

            }

        }
        [HttpPost]
        [Route("CreateCompare/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCompare(int id)
        {
            var idUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idUser == null)
            {

                return RedirectToAction("Login", "User");
            }
            if (db.Compares.Any(d => d.IdUser == int.Parse(idUser) && d.IdProduct == id))
            {
                TempData["Exist"] = "this Product is already exists in Compare";
                return RedirectToAction("Detail", new { id = id });
            }
            var result = commentService.CreateCompare(id);
            if (result)
            {
                TempData["Success"] = "Compare Created Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to create comment";
            }
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        [Route("CreateComment/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateComment(int id, CreateComment createComment, CreateRating createRating)
        {

            var idUser = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (idUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            var result = commentService.CreateComment(createComment, createRating, id);

            if (result)
            {
                TempData["Message"] = "Comment created successfully";
            }
            else
            {
                TempData["Error"] = "Failed to create comment";
            }
            return RedirectToAction("Detail", new { id = id });
        }


    }
}
