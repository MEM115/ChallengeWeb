using System.ComponentModel.DataAnnotations;

namespace ChallengeWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Card Number is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card Number must be 16 characters long")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card Number must contain only digits")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "PIN is required")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "PIN must be 4 characters long")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must contain only digits")]
        public string Pin { get; set; }
    }
}
