using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Entities;

namespace FIAP.TechChallenge.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Customer : Entity
{
	public string Document { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
}