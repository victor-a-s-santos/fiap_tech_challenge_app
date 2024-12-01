using FIAP.Crosscutting.Domain.Controller;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.Api.Controllers
{
    [Route("v{version:apiVersion}/products", Name = "Gestão de produtos")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ApiController
    {
        private readonly IProductServiceApp _productServiceApp;

        public ProductController(
            IProductServiceApp productServiceApp,
            IMediatorHandler mediator) : base(mediator)
        {
            _productServiceApp = productServiceApp;
        }

        /// <summary>
        /// Realiza a consulta dos produtos por categoria
        /// </summary>
        /// <param name="category">Categoria do produto (Lanche, Acompanhamento, Bebida ou Sobremesa)</param>
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("category/{category}")]
        public async Task<IActionResult> Get([FromRoute] string category)
        {
            var response = await _productServiceApp.GetProductsByCategory(category);

            return Response(response);
        }

        /// <summary>
        /// Realiza o cadastro de um novo produto
        /// </summary>
        /// <param name="viewModel">Dados do produto</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductViewModel viewModel)
        {
            await _productServiceApp.SaveProduct(viewModel);

            return Response();
        }

        /// <summary>
        /// Realiza a atualização de um produto
        /// </summary>
        /// <param name="viewModel">Dados do produto</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductViewModel viewModel)
        {
            await _productServiceApp.SaveProduct(viewModel, update: true);

            return Response();
        }

        /// <summary>
        /// Realiza a exclusão de um produto
        /// </summary>
        /// <param name="id">Código do produto</param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (ValidateStringToGuidParams(id))
                await _productServiceApp.RemoveProduct(id);

            return Response();
        }
    }
}
