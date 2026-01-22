namespace E_Commerce.Data.Repositories
{
    public class ProductRepository : CommonRepository<AllProduct>, IProductRepository
    {
        public ProductRepository(ShopperDBContext dbContext) : base(dbContext)
        {
        }
    }
}
