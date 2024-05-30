using Autofac;
using Microsoft.Extensions.Hosting;

namespace LePickaOrders.Infrastructure.MessageBus
{
    public class MessageBusModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventProcessor>()
                .As<IEventProcessor>()
                .SingleInstance();

            builder.RegisterType<MessageBusSubscriber>()
                .As<IHostedService>()
                .SingleInstance();
        }
    }
}
