using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers.Order
{
    public class RemoveOrderCommandHandler : MediatorCommandHandler<RemoveOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveOrderCommandHandler(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(RemoveOrderCommand request)
        {
            var registeredOrder = await _orderRepository.ExistsByExpressionAsync(x => x.Id == request.Id);

            if (!registeredOrder)
            {
                NotifyError("O pedido informado não foi encontrado");
                return;
            }

            await _orderItemRepository.DeleteByExpressionAsync(x => x.OrderId == request.Id);
            await _orderRepository.DeleteByIdAsync(request.Id);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
