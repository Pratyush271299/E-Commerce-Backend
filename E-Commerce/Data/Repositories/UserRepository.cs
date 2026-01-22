namespace E_Commerce.Data.Repositories
{
    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        public UserRepository(ShopperDBContext dbContext) : base(dbContext)
        {
        }
    }
}
