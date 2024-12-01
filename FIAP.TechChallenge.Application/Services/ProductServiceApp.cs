using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Queries;

namespace FIAP.TechChallenge.Application.Services
{
    public class ProductServiceApp : IProductServiceApp
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;

        public ProductServiceApp(
            IMediatorHandler mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<List<ProductViewModel>> GetProductsByCategory(string category)
        {
            var query = new GetProductsByCategoryQuery { Category = category };
            var response = await _mediator.SendQuery(query);

            return _mapper.Map<List<ProductViewModel>>(response);
        }

        public async Task SaveProduct(ProductViewModel viewModel, bool update = false)
        {
            if (!update)
            {
                var command = _mapper.Map<AddProductCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
            else
            {
                var command = _mapper.Map<UpdateProductCommand>(viewModel);
                await _mediator.SendCommand(command);
            }
        }

        public async Task RemoveProduct(string id)
        {
            var command = new RemoveProductCommand { Id = Guid.Parse(id) };
            await _mediator.SendCommand(command);
        }
    }
}
