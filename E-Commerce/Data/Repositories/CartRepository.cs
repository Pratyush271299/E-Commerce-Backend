namespace E_Commerce.Data.Repositories
{
    public class CartRepository : CommonRepository<CartProduct>, ICartRepository
    {
        public CartRepository(ShopperDBContext dbContext) : base(dbContext)
        {
        }
    }
}
