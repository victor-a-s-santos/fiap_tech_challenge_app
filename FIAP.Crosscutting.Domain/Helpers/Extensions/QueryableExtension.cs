using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.Crosscutting.Domain.Helpers.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TEntity> IncludeProperties<TEntity>(this IQueryable<TEntity> entities,
            params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        {
            if (properties != null)
                entities = properties.Aggregate(entities, (current, include) => current.Include(include));

            return entities;
        }
    }
}
