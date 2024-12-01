using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.Application.ViewModels
{
    public class OrderResponseViewModel
    {
        /// <summary>
        /// Código do pedido
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Valor total do pedido
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Lista de produtos do pedido
        /// </summary>
        public List<OrderItemViewModel> OrderItems { get; set; }

        /// <summary>
        /// Situação do pedido
        /// </summary>
        public string Situation { get; set; }

        /// <summary>
        /// Data de criação do pedido
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItemViewModel
    {
        /// <summary>
        /// Código do produto
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantida do produto
        /// </summary>
        public int Quantity { get; set; }
    }
}
