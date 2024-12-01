using FIAP.Crosscutting.Domain.Controller;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.Api.Controllers
{
    [Route("v{version:apiVersion}/customers", Name = "Gestão de clientes")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerServiceApp _customerServiceApp;

        public CustomerController(
            ICustomerServiceApp customerServiceApp,
            IMediatorHandler mediator) : base(mediator)
        {
            _customerServiceApp = customerServiceApp;
        }

        /// <summary>
        /// Realiza a consulta paginada de clientes
        /// </summary>
        /// <param name="page">Página atual</param>
        /// <param name="take">Quantidade de registros por página</param>
        /// <param name="order_property">Propriedade de ordenação</param>
        /// <param name="order_desc">Sentido da ordenação (desc)</param>
        [ProducesResponseType(typeof(PagedResult<CustomerResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int page = 1,
            [FromQuery] int take = 20,
            [FromQuery] string order_property = "created_at",
            [FromQuery] bool order_desc = true)
        {
            var response = await _customerServiceApp.GetPagedCustomers(page, take, order_property, order_desc);

            return Response(response);
        }

        /// <summary>
        /// Realiza a consulta de um cliente por código
        /// </summary>
        /// <param name="id">Código do cliente</param>
        [ProducesResponseType(typeof(CustomerResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (ValidateStringToGuidParams(id))
            {
                var response = await _customerServiceApp.GetCustomer(id);

                return Response(response);
            }

            return Response();
        }

        /// <summary>
        /// Realiza a consulta do cliente por CPF
        /// </summary>
        /// <param name="document">CPF do cliente</param>
        [ProducesResponseType(typeof(CustomerResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("document/{document}")]
        public async Task<IActionResult> GetByDocument([FromRoute] string document)
        {
            var response = await _customerServiceApp.GetCustomerByDocument(document);

            return Response(response);
        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente
        /// </summary>
        /// <param name="viewModel">Dados do cliente</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerRequestViewModel viewModel)
        {
            await _customerServiceApp.SaveCustomer(viewModel);

            return Response();
        }

        /// <summary>
        /// Realiza a atualização de um cliente
        /// </summary>
        /// <param name="viewModel">Dados do cliente</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerRequestViewModel viewModel)
        {
            await _customerServiceApp.SaveCustomer(viewModel, update: true);

            return Response();
        }

        /// <summary>
        /// Realiza a exclusão de um cliente
        /// </summary>
        /// <param name="id">Código do cliente</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (ValidateStringToGuidParams(id))
                await _customerServiceApp.RemoveCustomer(id);

            return Response();
        }
    }
}
