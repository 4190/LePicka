using LePickaProducts.Domain.Products;
using MediatR;


namespace LePickaProducts.Application.Queries.Products
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {

    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAll();

            return products;
        }
    }


}
