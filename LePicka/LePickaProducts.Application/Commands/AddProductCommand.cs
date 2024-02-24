using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Commands
{
    public class AddProductCommand : IRequest<Product>
    {
        public AddProductCommand(string name, string description, string category, decimal price) 
        {
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Product>
    {
        private readonly IProductRepository _repository;

        public AddProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product prod = new Product();
            prod.Name = request.Name;
            prod.Description = request.Description;
            prod.Price = request.Price;
            prod.Category = request.Category;

            return await _repository.Add(prod);
        }
    }
}
