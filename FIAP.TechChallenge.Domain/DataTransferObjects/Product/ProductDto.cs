using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.Domain.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}