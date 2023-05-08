using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace AuctionWebApplication.ViewModel
{
    public class RegisterViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name="Пароль")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Compare("Password", ErrorMessage="Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
