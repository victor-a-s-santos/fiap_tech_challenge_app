using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetPagedProductsQueryHandler : MediatorQueryHandler<GetPagedProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetPagedProductsQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public override async Task<PagedResult<ProductDto>> AfterValidation(GetPagedProductsQuery request)
        {
            var products = await _productRepository.PagedListAsync(request.Pagination);

            return _mapper.Map<PagedResult<ProductDto>>(products);
        }
    }
}
