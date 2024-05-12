using LePicka.Shared.Repository;
using LePickaOrders.Domain.Products;
using LePickaOrders.Domain.Users;

namespace LePickaOrders.Domain.Orders
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
