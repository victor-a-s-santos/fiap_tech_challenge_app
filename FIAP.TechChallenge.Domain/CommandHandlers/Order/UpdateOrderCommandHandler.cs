using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class UpdateOrderCommandHandler : MediatorCommandHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator,
            IOrderItemRepository orderItemRepository) : base(mediator)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderItemRepository = orderItemRepository;
        }

        public override async Task AfterValidation(UpdateOrderCommand request)
        {
            var order = await _orderRepository.GetOrderItemsById(request.Id);

            if (order == null)
            {
                NotifyError("O pedido informado não foi encontrado");
                return;
            }

            order.CustomerId = request.CustomerId;
            order.Total = request.Total!.Value;
            order.UpdatedAt = DateTime.Now.ToBrazilianTimezone();

            await _orderRepository.UpdateAsync(order);

            var removeOrderItemsProductsIds = order.OrderItems.Select(x => x.ProductId)
                                                              .Except(request.OrderItems.Select(x => x.ProductId));

            var newOrderItemsProductsIds = request.OrderItems.Select(x => x.ProductId)
                                                        .Except(order.OrderItems.Select(x => x.ProductId));

            foreach (var productId in removeOrderItemsProductsIds)
                await _orderItemRepository.DeleteByExpressionAsync(x => x.ProductId == productId);

            await _orderItemRepository.InsertRangeAsync(
                request.OrderItems.Where(x => newOrderItemsProductsIds.Contains(x.ProductId)).Select(x => new OrderItem()
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
