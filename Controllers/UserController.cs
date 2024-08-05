using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhinaMart.Helpers;
using PhinaMart.Models;
using PhinaMart.ViewModels;
using System.Security.Claims;

namespace PhinaMart.Controllers
{
    public class UserController : Controller
    {
        private readonly PhinaMartContext db;
        private readonly IMapper _mapper;

        public UserController(PhinaMartContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #region Login
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var uSer = db.Users.SingleOrDefault(us => us.Username == model.Username);
               
                if (uSer == null)
                {
                    ModelState.AddModelError("Error", "Cannot Username.");
                }
                else
                {
                    if (!uSer.Status)
                    {
                        ModelState.AddModelError("Error", "Usernamed block!,Please contact Admin");
                    }
                    else
                    {
                        if (uSer.Password != model.Password.ToMd5Hash(uSer.RandomKey))
                        {
                            ModelState.AddModelError("Error", "Username and Password false.");
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, uSer.Email),
                                new Claim(ClaimTypes.Name, uSer.Username),
                                new Claim(ClaimTypes.NameIdentifier, uSer.Id.ToString()),// Add this claim
                                new Claim("CustomerID", uSer.Id.ToString()),
                                new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {

                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVm model, IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uSer = _mapper.Map<User>(model);
                    uSer.RandomKey = MyUtil.GenerateRanDomKey();
                    uSer.Password = model.Password.ToMd5Hash(uSer.RandomKey);
                    uSer.Status = true; // sẽ xử lý khi dùng mail active
                    uSer.Role = 0;

                    if (Image != null)
                    {
                        uSer.Image = MyUtil.UploadImage(Image, "User");
                    }
                    db.Add(uSer);
                    db.SaveChanges();
                    return Redirect("Login");
                }
            }
            catch (Exception ex)
            {
                var mess = $"{ex.Message} shh";
            }
            return View();
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
