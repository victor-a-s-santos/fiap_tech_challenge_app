namespace FIAP.TechChallenge.Application.ViewModels
{
    public class CustomerRequestViewModel
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E-mail do cliente
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// CPF do cliente
        /// </summary>
        public string Document { get; set; }
    }
}
