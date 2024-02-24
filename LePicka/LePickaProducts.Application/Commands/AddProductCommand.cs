using AutoMapper;
using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Commands
{
    public class AddProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Product>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product prod = _mapper.Map<Product>(request);

            return await _repository.Add(prod);
        }
    }
}
