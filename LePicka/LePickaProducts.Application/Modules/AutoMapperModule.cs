using Autofac;
using AutoMapper;
using FluentValidation.Results;
using LePickaProducts.Application.Commands.Products;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;

namespace LePickaProducts.Application.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //source -> destination
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductResponse>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductDto
                    {
                        Id = src.Id,
                        Name = src.Name,
                        Description = src.Description,
                        Category = src.Category,
                        Price = src.Price
                    }));
                cfg.CreateMap<ValidationResult, ProductResponse>()
                    .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.ConvertAll(x => x.ErrorMessage)))
                    .ForMember(dest => dest.IsSucceeded, opt => opt.MapFrom(src => src.IsValid));
                cfg.CreateMap<AddProductCommand, Product>();
                cfg.CreateMap<EditProductCommand, Product>();
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
