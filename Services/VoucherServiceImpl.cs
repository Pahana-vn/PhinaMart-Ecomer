using PhinaMart.Models;
using PhinaMart.ViewModels;
using System.Text;

namespace PhinaMart.Services
{
    public class VoucherServiceImpl : VoucherService
    {
        private static Random Random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly PhinaMartContext context;
        public VoucherServiceImpl(PhinaMartContext context)
        {
            this.context = context;
        }

        private static string GenerateRandomCode(int length)
        {
            StringBuilder codeBuilder= new StringBuilder();
            for(int i = 0; i < length; i++) {
                codeBuilder.Append(chars[Random.Next(chars.Length)]);
            }
            return codeBuilder.ToString();
        }
        public bool CreateVoucher(AddVoucher addVoucher)
        {
            try
            {
                string randomCode = GenerateRandomCode(6);
                var Voucher = new Voucher
                {
                    Code = randomCode,
                    DiscountPercentage = addVoucher.DiscountPercentage,
                    StartDate = addVoucher.StartDate,
                    EndDate = addVoucher.EndDate,
                };
               context.Vouchers.Add(Voucher);
                return context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public dynamic GetVoucher()
        {
            return context.Vouchers.Select(d => new
            {
                Code=d.Code,
                DiscountPercentage=d.DiscountPercentage,    
                StartDate=d.StartDate,
                EndDate=d.EndDate,
            }).ToList();
        }
    }
}
