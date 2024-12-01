using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Application.Services
{
    public class CustomerServiceApp : ICustomerServiceApp
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;

        public CustomerServiceApp(
            IMediatorHandler mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerResponseViewModel>> GetPagedCustomers(int page, int take,
            string orderProperty, bool orderDesc)
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

            return _mapper.Map<PagedResult<CustomerResponseViewModel>>(response);
        }

        public async Task<CustomerResponseViewModel> GetCustomer(string id)
        {
            var query = new GetCustomerQuery { Id = Guid.Parse(id) };
            var response = await _mediator.SendQuery(query);

            return _mapper.Map<CustomerResponseViewModel>(response);
        }

        public async Task<CustomerResponseViewModel> GetCustomerByDocument(string document)
        {
            var query = new GetCustomerByDocumentQuery { Document = document };
            var response = await _mediator.SendQuery(query);

            return _mapper.Map<CustomerResponseViewModel>(response);
        }

        public async Task SaveCustomer(CustomerRequestViewModel viewModel, bool update = false)
        {
            if (!update)
            {
                var command = _mapper.Map<AddCustomerCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
            else
            {
                var command = _mapper.Map<UpdateCustomerCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
        }

        public async Task RemoveCustomer(string id)
        {
            var command = new RemoveCustomerCommand { Id = Guid.Parse(id) };
            await _mediator.SendCommand(command);
        }
    }
}
