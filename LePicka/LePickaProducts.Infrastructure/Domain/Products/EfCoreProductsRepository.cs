using LePickaProducts.Infrastructure.Database;
using LePickaProducts.Domain.Products;
using LePickaProducts.Infrastructure.DatabaseContext;

namespace LePickaProducts.Infrastructure.Domain.Products
{
    public class EfCoreProductsRepository : EfCoreRepository<Product, ApplicationDbContext>, IProductRepository
    {
        public EfCoreProductsRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
