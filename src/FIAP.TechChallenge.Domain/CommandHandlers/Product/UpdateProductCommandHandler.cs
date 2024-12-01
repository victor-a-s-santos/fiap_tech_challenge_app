using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class UpdateProductCommandHandler : MediatorCommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(UpdateProductCommand request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                NotifyError("O produto informado não foi encontrado");
                return;
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price!.Value;
            product.Category = request.Category;
            product.UpdatedAt = DateTime.Now.ToBrazilianTimezone();

            await _productRepository.UpdateAsync(product);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
