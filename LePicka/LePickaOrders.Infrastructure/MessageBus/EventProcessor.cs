using AutoMapper;
using LePickaOrders.Domain.Users;
using LePickaOrders.Application.Dtos.EventDtos;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace LePickaOrders.Infrastructure.MessageBus
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.AuthUserCreated:
                    await AddUser(message);
                    break;
                case EventType.ProductsProductEdited:
                    //UpdateProduct
                    break;
                default:
                    Console.WriteLine("--> Unknown event. Stopping processing");
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            switch(eventType.Event)
            {
                case "User_Created":
                    Console.WriteLine("--> User Created event detected!");
                    return EventType.AuthUserCreated;
                case "Product_Edit":
                    Console.WriteLine("--> Product Edit event detected!");
                    return EventType.ProductsProductEdited;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task AddUser(string userAddedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUsersRepository>();
                AuthUserAddedDto userAddedDto = JsonSerializer.Deserialize<AuthUserAddedDto>(userAddedMessage);

                try
                {
                    User user = _mapper.Map<User>(userAddedDto);
                    await repo.Add(user);
                    Console.WriteLine($"--> User from {user.DataSourceMicroserviceName} added to DB");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add User to DB: {ex.Message}");
                }
            }
        }


        enum EventType
        {
            AuthUserCreated,
            ProductsProductEdited,
            Undetermined
        }
    }
}
