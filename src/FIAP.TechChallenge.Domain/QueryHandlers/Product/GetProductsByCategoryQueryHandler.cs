using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetProductsByCategoryQueryHandler : MediatorQueryHandler<GetProductsByCategoryQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public override async Task<List<ProductDto>> AfterValidation(GetProductsByCategoryQuery request)
        {
            var products = await _productRepository.ListByExpressionAsync(x => x.Category.ToLower() == request.Category.ToLowerFormat());

            if (products?.Any() ?? false)
                products = products.OrderBy(x => x.Name).ToList();

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
