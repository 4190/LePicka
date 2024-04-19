using AutoMapper;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Queries.Products
{
    public class GetProductQuery : IRequest<ProductDto>
    {
      public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.Get(request.Id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}