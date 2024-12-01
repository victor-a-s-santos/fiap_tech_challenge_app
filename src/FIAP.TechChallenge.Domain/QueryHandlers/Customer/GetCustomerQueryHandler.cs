using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetCustomerQueryHandler : MediatorQueryHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerQueryHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public override async Task<CustomerDto> AfterValidation(GetCustomerQuery request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
