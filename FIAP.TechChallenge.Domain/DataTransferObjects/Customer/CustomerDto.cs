using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.Domain.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}