using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;

namespace LePickaProducts.Application.Commands.Products;

        public class EditProductCommand : IRequest<ProductResponse>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }

        public class EditProductCommandHandler : IRequestHandler<EditProductCommand, ProductResponse>
        {
            private readonly IProductRepository _repository;
            private readonly IMapper _mapper;
            private readonly IValidator<EditProductCommand> _validator;

            public EditProductCommandHandler(IProductRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ProductResponse> Handle(EditProductCommand request, CancellationToken cancellationToken)
            {

                var validationResult = await _validator.ValidateAsync(request);

                if (validationResult.IsValid)
                {
                    Product prod = _mapper.Map<Product>(request);
                    return _mapper.Map<ProductResponse>(await _repository.Update(prod));
                }
                else
                {
                    ProductResponse response = _mapper.Map<ProductResponse>(validationResult);

                    return response;
                }
            }
        }
