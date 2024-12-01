using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Commands;

[ExcludeFromCodeCoverage]
public abstract class OrderCommand : Command
{
    public Guid? CustomerId { get; set; }
    public decimal? Total { get; set; }
    public List<OrderItemCommand> OrderItems { get; set; }
}

[ExcludeFromCodeCoverage]
public class OrderItemCommand
{
    public Guid ProductId { get; set; }
    public int? Quantity { get; set; }
}