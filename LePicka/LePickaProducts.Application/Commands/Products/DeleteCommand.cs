using AutoMapper;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.Delete(request.Id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}