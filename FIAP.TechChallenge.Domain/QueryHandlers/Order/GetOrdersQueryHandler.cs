using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetOrdersQueryHandler : MediatorQueryHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediator) : base(mediator)
        {
            _orderRepository = orderRepository;
        }

        public override async Task<List<OrderDto>> AfterValidation(GetOrdersQuery request)
        {
            var response = await _orderRepository.GetOrders();

            return response;
        }
    }
}
