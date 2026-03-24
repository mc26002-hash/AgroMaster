using System.ComponentModel.DataAnnotations;

namespace Agromercado.AppMVC.Models
{
    public class LoginViewModel
    {
        [Required]
        public string? Correo { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}