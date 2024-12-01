using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Queries.Handlers;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Domain.QueryHandlers
{
    public class GetPagedCustomersQueryHandler : MediatorQueryHandler<GetPagedCustomersQuery, PagedResult<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetPagedCustomersQueryHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public override async Task<PagedResult<CustomerDto>> AfterValidation(GetPagedCustomersQuery request)
        {
            var customers = await _customerRepository.PagedListAsync(request.Pagination);

            return _mapper.Map<PagedResult<CustomerDto>>(customers);
        }
    }
}
