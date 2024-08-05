using PhinaMart.ViewModels;

namespace PhinaMart.Services
{
    public interface VoucherService
    {
        public bool CreateVoucher(AddVoucher addVoucher);
        public dynamic GetVoucher();
    }
}
