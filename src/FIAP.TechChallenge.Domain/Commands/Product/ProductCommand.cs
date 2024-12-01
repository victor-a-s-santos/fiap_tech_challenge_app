using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Commands;

[ExcludeFromCodeCoverage]
public abstract class ProductCommand : Command
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public string Category { get; set; }
}