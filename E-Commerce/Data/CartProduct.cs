namespace E_Commerce.Data
{
    public class CartProduct
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int AllProductId { get; set; }
        public AllProduct? AllProduct { get; set; }
    }
}
