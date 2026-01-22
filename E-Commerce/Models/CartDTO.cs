namespace E_Commerce.Models
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public int UserId { get; set; }
    }
}
