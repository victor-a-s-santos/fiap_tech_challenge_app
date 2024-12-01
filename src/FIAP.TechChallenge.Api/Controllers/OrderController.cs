using FIAP.Crosscutting.Domain.Controller;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.Api.Controllers
{
    [Route("v{version:apiVersion}/orders", Name = "Gestão de pedidos")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ApiController
    {
        private readonly IOrderServiceApp _orderServiceApp;

        public OrderController(
            IOrderServiceApp orderServiceApp,
            IMediatorHandler mediator) : base(mediator)
        {
            _orderServiceApp = orderServiceApp;
        }

        /// <summary>
        /// Realiza a consulta do pedidos
        /// </summary>
        [ProducesResponseType(typeof(List<OrderResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _orderServiceApp.GetOrders();

            return Response(response);
        }

        /// <summary>
        /// Realiza a consulta de um pedido por código
        /// </summary>
        /// <param name="id">Código do pedido</param>
        [ProducesResponseType(typeof(CustomerResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (ValidateStringToGuidParams(id))
            {
                var response = await _orderServiceApp.GetOrder(id);

                return Response(response);
            }

            return Response();
        }

        /// <summary>
        /// Realiza a consulta paginada dos pedidos por código do cliente
        /// </summary>
        /// <param name="customerId">Código do cliente</param>
        /// <param name="page">Página atual</param>
        /// <param name="take">Quantidade de registros por página</param>
        /// <param name="order_property">Propriedade de ordenação</param>
        /// <param name="order_desc">Sentido da ordenação (desc)</param>
        [ProducesResponseType(typeof(CustomerResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("customer")]
        public async Task<IActionResult> GetByCustomerId(
            [FromQuery] string customerId,
            [FromQuery] int page = 1,
            [FromQuery] int take = 20,
            [FromQuery] string order_property = "created_at",
            [FromQuery] bool order_desc = true)
        {
            var response = await _orderServiceApp.GetPagedOrdersByCustomerId(customerId, page, take, order_property, order_desc);

            return Response(response);
        }

        /// <summary>
        /// Realiza o cadastro de um novo pedido
        /// </summary>
        /// <param name="viewModel">Dados do cliente</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestViewModel viewModel)
        {
            await _orderServiceApp.SaveOrder(viewModel);

            return Response();
        }

        /// <summary>
        /// Realiza a atualização de um pedido
        /// </summary>
        /// <param name="viewModel">Dados do cliente</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderRequestViewModel viewModel)
        {
            await _orderServiceApp.SaveOrder(viewModel, update: true);

            return Response();
        }

        /// <summary>
        /// Realiza a exclusão de um pedido
        /// </summary>
        /// <param name="id">Código do pedido</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (ValidateStringToGuidParams(id))
                await _orderServiceApp.RemoveOrder(id);

            return Response();
        }
    }
}
