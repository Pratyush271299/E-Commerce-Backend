using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string? Role { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }
}
