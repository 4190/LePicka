using LePicka.Shared.Repository;
using System.ComponentModel.DataAnnotations;

namespace LePickaProducts.Domain.Products
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
