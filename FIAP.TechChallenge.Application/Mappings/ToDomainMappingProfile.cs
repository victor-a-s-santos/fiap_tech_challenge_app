using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Application.ViewModels;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Entities;
using static FIAP.TechChallenge.Domain.Commands.OrderCommand;

namespace FIAP.TechChallenge.Application.Mappings
{
    public class ToDomainMappingProfile : Profile
    {
        public ToDomainMappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<PagedResult<Customer>, PagedResult<CustomerDto>>();
            CreateMap<CustomerRequestViewModel, AddCustomerCommand>();
            CreateMap<CustomerRequestViewModel, UpdateCustomerCommand>();

            CreateMap<ProductViewModel, AddProductCommand>();
            CreateMap<ProductViewModel, UpdateProductCommand>();
            CreateMap<Product, ProductDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<PagedResult<Order>, PagedResult<OrderDto>>();
            CreateMap<OrderRequestViewModel, AddOrderCommand>();
            CreateMap<OrderRequestViewModel, UpdateOrderCommand>();
            CreateMap<OrderItemRequestViewModel, OrderItemCommand>();
        }
    }
}
