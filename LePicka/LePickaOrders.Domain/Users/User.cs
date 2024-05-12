using LePicka.Shared.Repository;

namespace LePickaOrders.Domain.Users
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
