using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetProductQueryHandler : MediatorQueryHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public override async Task<ProductDto> AfterValidation(GetProductQuery request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            return _mapper.Map<ProductDto>(product);
        }
    }
}
