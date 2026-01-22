using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class SignInDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
