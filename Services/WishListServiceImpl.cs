using Microsoft.CodeAnalysis;
using PhinaMart.Models;
using PhinaMart.ViewModels;
using System.Security.Claims;

namespace PhinaMart.Services
{
    public class WishListServiceImpl : WishlistService
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly PhinaMartContext _phinaMartContext;

        public WishListServiceImpl(IHttpContextAccessor contextAccessor, PhinaMartContext phinaMartContext)
        {
            _contextAccessor = contextAccessor;
            _phinaMartContext = phinaMartContext;
        }
        
        public bool AddToWishList( int productId)
        {
            using (var transaction = _phinaMartContext.Database.BeginTransaction())
            {
                try
                {
                    var Id = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (Id == null)
                    {
                        throw new Exception("User ID claim not found");
                    }
                    var UserId = int.Parse(Id.Value);
                    if (!_phinaMartContext.WishLists.Any(w => w.UserId == UserId && w.ProductId == productId))
                    {
                        var wishlist = new WishList
                        {
                            UserId = UserId,
                            ProductId = productId,
                            SelectDate = DateTime.Now
                        };
                        _phinaMartContext.WishLists.Add(wishlist);
                        _phinaMartContext.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
