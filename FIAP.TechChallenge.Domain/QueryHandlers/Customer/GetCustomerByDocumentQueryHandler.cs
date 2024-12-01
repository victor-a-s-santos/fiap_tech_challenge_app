using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetCustomerByDocumentQueryHandler : MediatorQueryHandler<GetCustomerByDocumentQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByDocumentQueryHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public override async Task<CustomerDto> AfterValidation(GetCustomerByDocumentQuery request)
        {
            var customer = await _customerRepository.GetFirstByExpressionAsync(x => x.Document == request.Document);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
