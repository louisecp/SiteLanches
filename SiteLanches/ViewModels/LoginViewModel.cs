using System.ComponentModel.DataAnnotations;

namespace SiteLanches.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter name")]
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }


    }
}
