using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhinaMart.Models;
using PhinaMart.Services;
using PhinaMart.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhinaMart.Controllers
{
    [Route("[controller]")]
    public class CompareController : Controller
    {
       private readonly PhinaMartContext context;
        private readonly CommentService commentService;
        public CompareController(PhinaMartContext context,CommentService commentService)
        {
            this.context = context;
            this.commentService = commentService;
        }
        private static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>",string.Empty);
        }
        [HttpPost("DeleteCompare/{id}")]
        public IActionResult DeleteCompare(int id)
        {
            var result=commentService.DeleteCompare(id);
            if (result)
            {
                TempData["Message"] = "Delete Compare successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Delete Compare";
            }
            return RedirectToAction("ComapreClient");
        }
        [HttpGet("ComapreClient")]
        public IActionResult Index()
        {
            
           
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return RedirectToAction("Login", "User");
            }
            var Compare = context.Compares.Where(d=>d.IdUser==int.Parse(Id)).Select(d => new
            {
                Id = d.Id,
                IdProduct = d.IdProduct,
                Name =d.IdProductNavigation.Name,
                Category=d.IdProductNavigation.Category.Name,
                Price=d.IdProductNavigation.Price,
                Url=d.IdProductNavigation.Image,
                Brand=d.IdProductNavigation.Brand.CompanyName,
                Description= StripHtml(d.IdProductNavigation.Description),
            }).ToList();
            ViewBag.Compares=Compare;

            return View();
        }

        
    }
}
