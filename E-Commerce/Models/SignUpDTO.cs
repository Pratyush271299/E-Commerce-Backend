using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class SignUpDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
