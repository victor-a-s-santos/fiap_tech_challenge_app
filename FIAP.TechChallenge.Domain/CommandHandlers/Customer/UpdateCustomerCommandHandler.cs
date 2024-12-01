using FIAP.Crosscutting.Domain.Commands.Handlers;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;

namespace FIAP.TechChallenge.Domain.CommandHandlers
{
    public class UpdateCustomerCommandHandler : MediatorCommandHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(UpdateCustomerCommand request)
        {
            var customer = await _customerRepository.GetFirstByExpressionAsync(x => x.Id == request.Id);

            if (customer == null)
            {
                NotifyError("O cliente informado não foi encontrado");
                return;
            }

            var customerWithSameDocumentOrEmail = await _customerRepository
                .ExistsByExpressionAsync(x => (x.Document == request.Document || x.Email == request.Email) && x.Id != request.Id);

            if (customerWithSameDocumentOrEmail)
            {
                NotifyError("Já existe um cliente com o CPF ou e-mail informado");
                return;
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Document = request.Document;
            customer.UpdatedAt = DateTime.Now.ToBrazilianTimezone();

            await _customerRepository.UpdateAsync(customer);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
