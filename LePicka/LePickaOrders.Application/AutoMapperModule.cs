using Autofac;
using AutoMapper;
using LePickaOrders.Application.Dtos;
using LePickaOrders.Domain.Products;

namespace LePickaOrders.Application
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
            })).AsSelf().SingleInstance();


            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            }).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
