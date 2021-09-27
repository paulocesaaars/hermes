using System.ComponentModel.DataAnnotations;

namespace Deviot.Hermes.Application.ViewModels
{
    public class UserPasswordViewModel : EntityViewModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string Password { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string NewPassword { get; set; }
    }
}
