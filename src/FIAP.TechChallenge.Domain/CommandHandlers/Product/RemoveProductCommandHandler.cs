using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class RemoveProductCommandHandler : MediatorCommandHandler<RemoveProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(RemoveProductCommand request)
        {
            var registeredProduct = await _productRepository.ExistsByExpressionAsync(x => x.Id == request.Id);

            if(!registeredProduct)
            {
                NotifyError("O produto informado não foi encontrado");
                return;
            }

            await _productRepository.DeleteByIdAsync(request.Id);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
