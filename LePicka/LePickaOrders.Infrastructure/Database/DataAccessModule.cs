using Autofac;
using LePickaOrders.Domain.Orders;
using LePickaOrders.Domain.Products;
using LePickaOrders.Domain.Users;
using LePickaOrders.Infrastructure.Repository;

namespace LePickaOrders.Infrastructure.Database
{
    public class DataAccessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfCoreProductsRepository>()
                .As<IProductsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfCoreUsersRepository>()
                .As<IUsersRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfCoreOrdersRepository>()
                .As<IOrdersRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
