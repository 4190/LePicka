using AutoMapper;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;

namespace LePickaProducts.Application.Queries.Products
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {

    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAll();
           

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
