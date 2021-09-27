using System.ComponentModel.DataAnnotations;

namespace Deviot.Hermes.Application.ViewModels
{
    public class UserInfoViewModel : EntityViewModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string UserName { get; set; }

        public bool Enabled { get; set; } = false;

        public bool Administrator { get; set; } = false;
    }
}
