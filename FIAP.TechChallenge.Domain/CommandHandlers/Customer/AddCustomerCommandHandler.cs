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
    public class AddCustomerCommandHandler : MediatorCommandHandler<AddCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task AfterValidation(AddCustomerCommand request)
        {
            var registeredCustomer = await _customerRepository
                .ExistsByExpressionAsync(x => x.Document == request.Document || x.Email == request.Email);

            if (registeredCustomer)
            {
                NotifyError("Já existe um cliente com o CPF ou e-mail informado");
                return;
            }

            var customer = new Customer
            {
                Name = request.Name.Capitalize(),
                Document = request.Document,
                Email = request.Email.ToLowerFormat(),
            };

            await _customerRepository.InsertAsync(customer);

            if (HasNotification() || !await _unitOfWork.CommitAsync())
                NotifyError(Values.Message.DefaultError);
        }
    }
}
