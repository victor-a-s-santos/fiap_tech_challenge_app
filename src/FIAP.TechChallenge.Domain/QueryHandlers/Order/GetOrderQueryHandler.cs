using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetOrderQueryHandler : MediatorQueryHandler<GetOrderQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrderQueryHandler(
            IMapper mapper,
            IMediatorHandler mediator,
            IOrderRepository orderRepository) : base(mediator)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async override Task<OrderDto> AfterValidation(GetOrderQuery request)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);

            return _mapper.Map<OrderDto>(order);
        }
    }
}
