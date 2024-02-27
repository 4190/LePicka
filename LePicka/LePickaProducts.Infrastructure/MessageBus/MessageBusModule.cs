using Autofac;
using LePickaProducts.Domain.Products;
using LePickaProducts.Infrastructure.Domain.Products;

namespace LePickaProducts.Infrastructure.MessageBus
{
    public class MessageBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageBusClient>()
                .As<IMessageBusClient>()
                .InstancePerLifetimeScope();


        }
    }
}
