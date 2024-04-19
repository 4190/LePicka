using Autofac;
using LePickaProducts.Application.Queries.Products;


namespace LePickaProducts.Application
{
    public class QueryCommandRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetAllProductsQuery>().AsImplementedInterfaces();
            builder.RegisterType<GetProductsQuery>().AsImplementedInterfaces();
        }
    }
}
