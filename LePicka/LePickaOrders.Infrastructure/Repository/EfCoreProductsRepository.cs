using LePickaOrders.Domain.Products;
using LePickaOrders.Infrastructure.Database;

namespace LePickaOrders.Infrastructure.Repository
{
    public class EfCoreProductsRepository : EfCoreRepository<Product, ApplicationDbContext>, IProductsRepository
    {
        public EfCoreProductsRepository(ApplicationDbContext context) : base(context) { }
    }
}
