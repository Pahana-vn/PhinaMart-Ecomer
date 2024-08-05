using Microsoft.AspNetCore.Mvc;
using PhinaMart.Services;
using PhinaMart.ViewModels;

namespace PhinaMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/voucher")]
    public class VoucherController : Controller
    {
        private readonly VoucherService _voiceService;
        public VoucherController(VoucherService voiceService)
        {
            _voiceService = voiceService;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.voucher=_voiceService.GetVoucher();
            return View();
        }
        
        [Route("AddVoucher")]
        public IActionResult AddVoucher(AddVoucher addVoucher)
        {
            try
            {
                var result=_voiceService.CreateVoucher(addVoucher);
                if (result)
                {
                    TempData["Success"] = "Voucher Created Successfully";

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
