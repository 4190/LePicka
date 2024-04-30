using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;
using MediatR;

namespace LePickaProducts.Application.Commands.Products;

        public class EditProductCommand : IRequest<ProductDto>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }

        public class EditProductCommandHandler : IRequestHandler<EditProductCommand, ProductDto>
        {
            private readonly IProductRepository _repository;
            private readonly IMapper _mapper;

            public EditProductCommandHandler(IProductRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(EditProductCommand request, CancellationToken cancellationToken)
            {
                Product prod = _mapper.Map<Product>(request);

                return _mapper.Map<ProductDto>(await _repository.Update(prod));
            }
        }
