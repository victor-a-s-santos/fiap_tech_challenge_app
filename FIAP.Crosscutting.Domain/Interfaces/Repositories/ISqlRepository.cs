using FIAP.Crosscutting.Domain.Entities;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using System.Linq.Expressions;

namespace FIAP.Crosscutting.Domain.Interfaces.Repositories
{
    public interface ISqlRepository<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination);
        Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, Expression<Func<TEntity, bool>> expression);
        Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IQueryable<TEntity>> ListAllAsync();
        Task<IList<TEntity>> ListAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IList<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task<IList<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task<long> CountAllAsync();
        Task<long> CountByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task DeleteByIdAsync(Guid id);
        Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task InsertOrUpdateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task InsertRangeAsync(IList<TEntity> entities);
        Task InsertAsync(TEntity entity);
        Task<bool> AnyAsync();
        Task<int> SaveChangesAsync();
    }
}
