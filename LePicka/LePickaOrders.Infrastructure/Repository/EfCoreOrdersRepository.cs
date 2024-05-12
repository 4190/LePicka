using LePickaOrders.Domain.Orders;
using LePickaOrders.Infrastructure.Database;

namespace LePickaOrders.Infrastructure.Repository
{
    public class EfCoreOrdersRepository : EfCoreRepository<Order, ApplicationDbContext>, IOrdersRepository
    {
        public EfCoreOrdersRepository(ApplicationDbContext context) : base(context) { }
    }

    //todo eager loading methods
}


