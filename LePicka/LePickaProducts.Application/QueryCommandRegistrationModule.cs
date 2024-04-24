using Autofac;
using LePickaProducts.Application.Commands.Products;
using LePickaProducts.Application.Queries.Products;



namespace LePickaProducts.Application
{
    public class QueryCommandRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetAllProductsQuery>().AsImplementedInterfaces();
            builder.RegisterType<GetProductQuery>().AsImplementedInterfaces();
            builder.RegisterType<DeleteProductCommand>().AsImplementedInterfaces();
        }
    }
}
