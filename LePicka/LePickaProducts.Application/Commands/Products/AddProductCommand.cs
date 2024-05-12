using AutoMapper;
using FluentValidation;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;
using System.ComponentModel;


namespace LePickaProducts.Application.Commands.Products
{
    public class AddProductCommand : IRequest<ProductResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddProductCommand> _validator;

        public AddProductCommandHandler(IProductRepository repository, IMapper mapper, IValidator<AddProductCommand> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ProductResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
        { 
            var validationResult = await _validator.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                Product prod = _mapper.Map<Product>(request);
                return _mapper.Map<ProductResponse>(await _repository.Add(prod));
            }
            else
            {
                ProductResponse response = _mapper.Map<ProductResponse>(validationResult);

                return response;
            }
        }
    }
}
