using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.Domain.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public string Situation { get; set; }
    public DateTime CreatedAt { get; set; }
}

[ExcludeFromCodeCoverage]
public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}