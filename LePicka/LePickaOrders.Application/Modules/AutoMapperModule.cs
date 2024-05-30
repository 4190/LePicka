using Autofac;
using AutoMapper;
using LePickaOrders.Application.Dtos;
using LePickaOrders.Application.Dtos.EventDtos;
using LePickaOrders.Domain.Products;
using LePickaOrders.Domain.Users;

namespace LePickaOrders.Application.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<AuthUserAddedDto, User>()
                    .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
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
