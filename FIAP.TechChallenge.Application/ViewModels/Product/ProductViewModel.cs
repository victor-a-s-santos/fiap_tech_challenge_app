namespace FIAP.TechChallenge.Application.ViewModels
{
    public class ProductViewModel
    {
        /// <summary>
        /// Código do produto
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descrição do produto
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Preço do produto
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Categoria do produto (Lanche, Acompanhamento, Bebida ou Sobremesa)
        /// </summary>
        public string Category { get; set; }
    }
}
