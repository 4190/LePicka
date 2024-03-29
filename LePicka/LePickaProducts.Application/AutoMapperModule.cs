﻿using Autofac;
using AutoMapper;
using LePickaProducts.Application.Commands.Products;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;

namespace LePickaProducts.Application
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddProductRequest, AddProductCommand>();
                cfg.CreateMap<AddProductCommand, Product>();
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
