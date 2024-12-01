using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetPagedOrdersByDocumentQueryHandler : MediatorQueryHandler<GetPagedOrdersQuery, PagedResult<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetPagedOrdersByDocumentQueryHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async override Task<PagedResult<OrderDto>> AfterValidation(GetPagedOrdersQuery request)
        {
            var orders = await _orderRepository.PagedListAsync(request.Pagination, x => x.CustomerId == request.CustomerId);

            return _mapper.Map<PagedResult<OrderDto>>(orders);
        }
    }
}
