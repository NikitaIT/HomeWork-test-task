using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Identity
{
    public class LoginViewModel
    {
        [Display(Name = "User name")]
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}