using System.ComponentModel.DataAnnotations;

namespace PhinaMart.ViewModels
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Please enter id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }

        public bool Gender { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "No format phone +84")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "No format email")]
        public string Email { get; set; }

        public string? Image { get; set; }
    }
}
