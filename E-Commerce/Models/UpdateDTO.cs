namespace E_Commerce.Models
{
    public class UpdateDTO
    {
        public int UserId { get; set; }
        public int AllProductId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public string? Size { get; set; }
    }
}
