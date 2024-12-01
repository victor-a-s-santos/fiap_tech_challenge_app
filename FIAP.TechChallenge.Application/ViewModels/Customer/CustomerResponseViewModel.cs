using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.Application.ViewModels
{
    public class CustomerResponseViewModel
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CPF do cliente
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// E-mail do cliente
        /// </summary>
        public string Email { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
