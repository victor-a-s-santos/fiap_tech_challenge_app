using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Enumerators;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class AddOrderCommandHandler : MediatorCommandHandler<AddOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddOrderCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator,
            IOrderItemRepository orderItemRepository,
            ICustomerRepository customerRepository) : base(mediator)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderItemRepository = orderItemRepository;
            _customerRepository = customerRepository;
        }

        public override async Task AfterValidation(AddOrderCommand request)
        {
            if (request.CustomerId.HasValue)
            {
                var customerExists = await _customerRepository.ExistsByExpressionAsync(x => x.Id == request.CustomerId);
                if (!customerExists)
                {
                    NotifyError("O código do cliente informado não foi encontrado");
                    return;
                }
            }

            Int64 lastOrderNumber = await _orderRepository.CountAllAsync();            

            var order = new Entities.Order
            {
                CustomerId = request.CustomerId,
                Number = lastOrderNumber + 1,
                Total = request.Total!.Value,
                OrderSituationEnum = OrderSituationEnum.Received
            };

            await _orderRepository.InsertAsync(order);

            await _orderItemRepository.InsertRangeAsync(request.OrderItems.Select(x => new OrderItem()
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity!.Value,
            }).ToList());

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
