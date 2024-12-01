using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class AddProductCommandHandler : MediatorCommandHandler<AddProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(AddProductCommand request)
        {
            var registeredProduct = await _productRepository
                .GetFirstByExpressionAsync(x => x.Name.ToLower() == request.Name.ToLower()
                    && x.Category == request.Category);

            if (registeredProduct != null)
            {
                NotifyError("O produto informado já existe");
                return;
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price!.Value,
                Category = request.Category
            };

            await _productRepository.InsertAsync(product);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
