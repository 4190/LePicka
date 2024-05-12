using Autofac;
using FluentValidation;
using LePickaProducts.Application.Commands.Products;
using LePickaProducts.Application.Validators;

namespace LePickaProducts.Application.Modules
{
    public class FluentValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddProductCommandValidator>()
                .As<IValidator<AddProductCommand>>()
                .InstancePerLifetimeScope();
        }
    }
}
