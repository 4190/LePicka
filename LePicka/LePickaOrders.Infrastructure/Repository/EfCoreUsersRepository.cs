using LePickaOrders.Domain.Users;
using LePickaOrders.Infrastructure.Database;

namespace LePickaOrders.Infrastructure.Repository
{
    public class EfCoreUsersRepository : EfCoreRepository<User, ApplicationDbContext>, IUsersRepository
    {
        public EfCoreUsersRepository(ApplicationDbContext context) : base(context) { }
    }
}
