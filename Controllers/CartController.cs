using Microsoft.AspNetCore.Mvc;
using PhinaMart.Models;
using PhinaMart.ViewModels;
using PhinaMart.Helpers;
using Microsoft.AspNetCore.Authorization;
using PhinaMart.Services;

namespace PhinaMart.Controllers
{
    public class CartController : Controller
    {
        private readonly PhinaMartContext db;
        private readonly PaypalClient _paypalClient;
        private readonly IVnPayService _vnPayservice;
       

        public CartController(PhinaMartContext context, PaypalClient paypalClient, IVnPayService vnPayservice)
        {
            db = context;
            _paypalClient = paypalClient;
            _vnPayservice = vnPayservice;
         
        }


        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        public IActionResult Index()
        {
           
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                var proDuct = db.Products.SingleOrDefault(p => p.Id == id);
                if (proDuct == null)
                {
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    Id = proDuct.Id,
                    Name = proDuct.Name,
                    Price = proDuct.Price ,
                    Image = proDuct.Image ?? string.Empty,
                    Quantity = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            var gioHang = Cart;
            foreach (var item in gioHang)
            {
                if (quantities.ContainsKey(item.Id))
                {
                    item.Quantity = quantities[item.Id];
                }
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Set(MySetting.CART_KEY, new List<CartItem>());
            return RedirectToAction("Index");
        }
        [Route("SearchVoucher")]
        [HttpPost]
        public IActionResult SearchVoucher(string coupon_code)
        {
            try
            {
                var Current = DateOnly.FromDateTime(DateTime.Now);
                // Find the voucher based on the coupon code
                var voucher = db.Vouchers.FirstOrDefault(v => v.Code == coupon_code && Current>=v.StartDate && Current<=v.EndDate);
                Console.WriteLine(voucher);
                // Calculate the total order amount from the cart
                float originalTotal = CalculateCartTotal();

                // Apply discount if a valid voucher is found
                if (voucher != null)
                {
                    float discountPercentage = voucher.DiscountPercentage;
                    float discountAmount = originalTotal * (discountPercentage / 100);
                    float finalTotal = originalTotal - discountAmount;

                    ViewBag.DiscountAmount = discountAmount;
                    ViewBag.DiscountPercentage = discountPercentage;
                    ViewBag.Total = finalTotal;
                }
                else
                {
                    TempData["Error"] = "Voucher Is Not Exists";
                    ViewBag.Total = originalTotal;
                }

                // Store PayPal client ID in ViewBag
                ViewBag.PaypalClientId = _paypalClient.ClientId;

                // Redirect to the Checkout action
                return RedirectToAction("Checkout",Cart);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Optionally, set an error message in ViewBag or TempData to show in the view
                ViewBag.ErrorMessage = "An error occurred while applying the voucher. Please try again.";

                // Redirect to the cart page or handle the error as needed
                return RedirectToAction("Cart");
            }
        }

        private float CalculateCartTotal()
        {
            // Calculate total order amount from the cart items
            float total = 0.0f;
            foreach (var item in Cart)
            {
                total += (float)item.Price * item.Quantity;
            }
            return total;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
        float total = 0.0f;
            foreach (var item in Cart)
            {

                total += (float)item.Price * item.Quantity;
            }
            ViewBag.PaypalClientdId = _paypalClient.ClientId;
            ViewBag.Total = total;
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model, string payment = "COD",decimal total=0.0m)
        {
            if (ModelState.IsValid)
            {
                if (payment == "Thanh toán VNPay")
                {
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = Cart.Sum(p => p.IntoMoney),
                        CreatedDate = DateTime.Now,
                        Description = $"{model.UserName} {model.Phone}",
                        FullName = model.UserName,
                        OrderId = new Random().Next(1000, 100000)
                    };
                    return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                }

                var customerId = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value);
                var khachHang = new User();
                if (model.GiongKhachHang)
                {
                    khachHang = db.Users.SingleOrDefault(kh => kh.Id == customerId);
                }

                var hoadon = new Order
                {
                    UserId = customerId,
                    UserName = model.UserName ?? khachHang.Username,
                    Address = model.Address ?? khachHang.Address,
                    Phone = model.Phone ?? khachHang.Phone,
                    OrderDate = DateTime.Now,
                    HowToPay = "COD",
                    HowToTransport = "GRAB",
                    Status = 0,
                    Note = model.Note,
                    TotalAmount = total // Assuming you want to set the total amount
                };

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Add(hoadon);
                        db.SaveChanges();

                        var cthds = new List<OrderDetail>();
                        foreach (var item in Cart)
                        {
                            cthds.Add(new OrderDetail
                            {
                                OrderId = hoadon.Id,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                ProductId = item.Id,
                                Discount = 0
                            });
                        }
                        db.AddRange(cthds);
                        db.SaveChanges();

                        transaction.Commit();

                        HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                        return View("Success");
                    }
                    catch
                    {
                        transaction.Rollback();
                        // Log the error here for debugging if needed
                        ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                    }
                }
            }
            return View(Cart);
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View("Fail");
        }

        #region Payment Paypal
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            //Thông tin đơn hàng gửi qua Paypal
            var tongTien = Cart.Sum(p => p.IntoMoney).ToString();
            var donViTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                //lưu đơn hàng vao database của mình
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        #endregion

        #region Payment VnPay
        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


            // Lưu đơn hàng vô database

            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");
        }
        #endregion
    }
}
