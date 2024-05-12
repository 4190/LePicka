using Autofac;
using LePickaProducts.Domain.Products;
using LePickaProducts.Infrastructure.Domain.Products;

namespace LePickaProducts.Infrastructure.Database
{
    public class DataAccessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfCoreProductsRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
