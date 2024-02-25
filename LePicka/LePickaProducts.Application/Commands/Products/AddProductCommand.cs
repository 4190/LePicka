using AutoMapper;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Commands.Products
{
    public class AddProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product prod = _mapper.Map<Product>(request);

            return _mapper.Map<ProductDto>(await _repository.Add(prod));
        }
    }
}
