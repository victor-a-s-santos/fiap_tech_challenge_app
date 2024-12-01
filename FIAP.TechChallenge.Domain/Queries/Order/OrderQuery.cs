using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.Queries;

namespace FIAP.TechChallenge.Domain.Queries;

[ExcludeFromCodeCoverage]
public abstract class OrderQuery<TResponse> : Query<TResponse>
{
    public PaginationObject Pagination { get; set; }
    public Guid CustomerId { get; set; }
}