using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.Application.ViewModels
{
    public class OrderRequestViewModel
	{
        /// <summary>
        /// Código do pedido
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        [JsonPropertyName("customer_id")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Valor total do pedido
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Lista de produtos do pedido
        /// </summary>
        [JsonPropertyName("items")]
        public List<OrderItemRequestViewModel> OrderItems { get; set; }
    }

    public class OrderItemRequestViewModel
    {
        /// <summary>
        /// Código do produto
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantidade do produto
        /// </summary>
        public int Quantity { get; set; }
    }
}
