using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Application.Services
{
    public class OrderServiceApp : IOrderServiceApp
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;

        public OrderServiceApp(
            IMediatorHandler mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<OrderResponseViewModel> GetOrder(string id)
        {
            var query = new GetOrderQuery { Id = Guid.Parse(id) };
            var response = await _mediator.SendQuery(query);

            return _mapper.Map<OrderResponseViewModel>(response);
        }

        public async Task<List<OrderResponseViewModel>> GetOrders()
        {
            var query = new GetOrdersQuery();
            var response = await _mediator.SendQuery(query);

            return _mapper.Map<List<OrderResponseViewModel>>(response);
        }

        public async Task<PagedResult<OrderResponseViewModel>> GetPagedOrdersByCustomerId(string customerId, int page, int take, string orderProperty, bool orderDesc)
        {
            var query = new GetPagedOrdersQuery
            {
                CustomerId = Guid.Parse(customerId),
                Pagination = new PaginationObject
                {
                    Page = page,
                    Take = take,
                    OrderProperty = orderProperty,
                    OrderDesc = orderDesc
                }
            };

            var response = await _mediator.SendQuery(query);

            return _mapper.Map<PagedResult<OrderResponseViewModel>>(response);
        }

        public async Task<PagedResult<OrderResponseViewModel>> GetPagedOrders(int page, int take, string orderProperty, bool orderDesc)
        {
            var query = new GetPagedCustomersQuery
            {
                Pagination = new PaginationObject
                {
                    Page = page,
                    Take = take,
                    OrderProperty = orderProperty,
                    OrderDesc = orderDesc
                }
            };

            var response = await _mediator.SendQuery(query);

            return _mapper.Map<PagedResult<OrderResponseViewModel>>(response);
        }

        public async Task SaveOrder(OrderRequestViewModel viewModel, bool update = false)
        {
            if (!update)
            {
                var command = _mapper.Map<AddOrderCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
            else
            {
                var command = _mapper.Map<UpdateOrderCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
        }

        public async Task RemoveOrder(string id)
        {
            var command = new RemoveOrderCommand { Id = Guid.Parse(id) };
            await _mediator.SendCommand(command);
        }
    }
}

