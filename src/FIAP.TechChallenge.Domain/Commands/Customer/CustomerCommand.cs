using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Commands;

[ExcludeFromCodeCoverage]
public abstract class CustomerCommand : Command
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
}