using System.ComponentModel.DataAnnotations;

namespace PhinaMart.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Please enter username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
