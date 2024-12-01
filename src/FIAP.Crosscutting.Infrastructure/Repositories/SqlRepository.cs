using FIAP.Crosscutting.Domain.Entities;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.Crosscutting.Infrastructure.Repositories
{
    public abstract class SqlRepository<TEntity> : ISqlRepository<TEntity> where TEntity : class, IEntity, new()
    {
        #region Properties

        public DbContext Context;

        #endregion

        #region Constructors e Destructors

        protected SqlRepository() { }

        protected SqlRepository(DbContext context)
        {
            Context = context;
        }

        ~SqlRepository()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        public DbSet<TEntity> DbSet()
        {
            return Context.Set<TEntity>();
        }

        public async Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination)
        {
            string orderProp = typeof(TEntity).GetProperty(pagination.OrderProperty.ToModelFormat()).Name;

            var type = typeof(TEntity);
            var prop = type.GetProperty(orderProp);
            var param = Expression.Parameter(type);

            var orderExpression = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)),
                param
            );

            var pagedResult = new PagedResult<TEntity>
            {
                CurrentPage = pagination.Page,
                PageSize = pagination.Take,
                TotalRecords = DbSet().Count()
            };

            int skip = (pagination.Page - 1) * pagination.Take;

            if (pagination.OrderDesc)
            {
                var data = DbSet().Any()
                    ? await Task.Run(() => DbSet()
                        .OrderByDescending(orderExpression)
                        .Skip(skip)
                        .Take(pagination.Take)
                        .AsNoTracking()
                        .ToList())
                    : null;

                pagedResult.Results = data;
            }
            else
            {
                var data = DbSet().Any()
                ? await Task.Run(() => DbSet()
                    .OrderBy(orderExpression)
                    .Skip(skip)
                    .Take(pagination.Take)
                    .AsNoTracking()
                    .ToList())
                : null;

                pagedResult.Results = data;
            }

            var pageCount = (double)pagedResult.TotalRecords / pagination.Take;
            pagedResult.PageCount = (int)Math.Ceiling(pageCount);

            return pagedResult;
        }

        public async Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, Expression<Func<TEntity, bool>> expression)
        {
            string orderProp = typeof(TEntity).GetProperty(pagination.OrderProperty.ToModelFormat()).Name;

            var type = typeof(TEntity);
            var prop = type.GetProperty(orderProp);

            var param = Expression.Parameter(type);
            var orderExpression = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)),
                param
            );

            var pagedResult = new PagedResult<TEntity>
            {
                CurrentPage = pagination.Page,
                PageSize = pagination.Take,
                TotalRecords = expression != null ? DbSet().Where(expression).Count() : DbSet().Count()
            };

            int skip = (pagination.Page - 1) * pagination.Take;

            if (pagination.OrderDesc)
            {
                var data = DbSet().Any(expression)
                        ? await Task.Run(() => DbSet()
                            .Where(expression)
                            .OrderByDescending(orderExpression)
                            .Skip(skip)
                            .Take(pagination.Take)
                            .AsNoTracking()
                            .ToList())
                        : null;

                pagedResult.Results = data;
            }
            else
            {
                var data = DbSet().Any(expression)
                        ? await Task.Run(() => DbSet()
                            .Where(expression)
                            .OrderBy(orderExpression)
                            .Skip(skip)
                            .Take(pagination.Take)
                            .AsNoTracking()
                            .ToList())
                        : null;

                pagedResult.Results = data;
            }

            var pageCount = (double)pagedResult.TotalRecords / pagination.Take;
            pagedResult.PageCount = (int)Math.Ceiling(pageCount);

            return pagedResult;
        }

        public async Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            string orderProp = typeof(TEntity).GetProperty(pagination.OrderProperty.ToModelFormat()).Name;

            var type = typeof(TEntity);
            var prop = type.GetProperty(orderProp);

            var param = Expression.Parameter(type);
            var orderExpression = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)),
                param
            );

            var pagedResult = new PagedResult<TEntity>
            {
                CurrentPage = pagination.Page,
                PageSize = pagination.Take,
                TotalRecords = expression != null ? DbSet().Where(expression).Count() : DbSet().Count()
            };

            int skip = (pagination.Page - 1) * pagination.Take;

            if (pagination.OrderDesc)
            {
                var data = DbSet().Any(expression)
                        ? await Task.Run(() => DbSet()
                            .Where(expression)
                            .OrderByDescending(orderExpression)
                            .Skip(skip)
                            .Take(pagination.Take)
                            .IncludeProperties(includeProperties)
                            .AsNoTracking()
                            .ToList()) : null;

                pagedResult.Results = data;
            }
            else
            {
                var data = DbSet().Any(expression)
                        ? await Task.Run(() => DbSet()
                            .Where(expression)
                            .OrderBy(orderExpression)
                            .Skip(skip)
                            .Take(pagination.Take)
                            .IncludeProperties(includeProperties)
                            .AsNoTracking()
                            .ToList()) : null;

                pagedResult.Results = data;
            }

            var pageCount = (double)pagedResult.TotalRecords / pagination.Take;
            pagedResult.PageCount = (int)Math.Ceiling(pageCount);

            return pagedResult;
        }

        public async Task<PagedResult<TEntity>> PagedListAsync(PaginationObject pagination, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            string orderProp = typeof(TEntity).GetProperty(pagination.OrderProperty.ToModelFormat()).Name;

            var type = typeof(TEntity);
            var prop = type.GetProperty(orderProp);

            var param = Expression.Parameter(type);
            var orderExpression = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)),
                param
            );

            var pagedResult = new PagedResult<TEntity>
            {
                CurrentPage = pagination.Page,
                PageSize = pagination.Take,
                TotalRecords = DbSet().Count()
            };

            int skip = (pagination.Page - 1) * pagination.Take;

            if (pagination.OrderDesc)
            {
                var data = DbSet().Any()
                       ? await Task.Run(() => DbSet()
                           .OrderByDescending(orderExpression)
                           .Skip(skip)
                           .Take(pagination.Take)
                           .IncludeProperties(includeProperties)
                           .AsNoTracking()
                           .ToList()) : null;

                pagedResult.Results = data;
            }
            else
            {
                var data = DbSet().Any()
                       ? await Task.Run(() => DbSet()
                           .OrderBy(orderExpression)
                           .Skip(skip)
                           .Take(pagination.Take)
                           .IncludeProperties(includeProperties)
                           .AsNoTracking()
                           .ToList()) : null;

                pagedResult.Results = data;
            }

            var pageCount = (double)pagedResult.TotalRecords / pagination.Take;
            pagedResult.PageCount = (int)Math.Ceiling(pageCount);

            return pagedResult;
        }

        public async Task<IQueryable<TEntity>> ListAllAsync()
        {
            return DbSet().Any()
                ? await Task.Run(() => DbSet().AsQueryable().AsNoTracking())
                : null;
        }

        public async Task<IList<TEntity>> ListAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return DbSet().Any()
                ? await DbSet().IncludeProperties(includeProperties).ToListAsync()
                : null;
        }

        public async Task<IList<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet().Any(expression) ? await DbSet().Where(expression).ToListAsync() : null;
        }

        public async Task<IList<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return DbSet().AsQueryable().Any(expression)
                ? await DbSet().Where(expression).IncludeProperties(includeProperties).ToListAsync()
                : null;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbSet().FindAsync(id);
        }

        public async Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet().SingleOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await DbSet().IncludeProperties(includeProperties).SingleOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet().FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await DbSet().IncludeProperties(includeProperties).FirstOrDefaultAsync(expression);
        }

        public async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet().AnyAsync(expression);
        }

        public async Task<long> CountAllAsync()
        {
            return await DbSet().AnyAsync() ? await DbSet().AsNoTracking().CountAsync() : 0;
        }

        public async Task<long> CountByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await ExistsByExpressionAsync(expression) ? await DbSet().AsNoTracking().CountAsync(expression) : 0;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var contextEntity = await GetByIdAsync(id);

            if (contextEntity == null) return;

            Context.Entry(contextEntity).State = EntityState.Deleted;

            DbSet().Remove(contextEntity);
        }

        public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            var items = await ListByExpressionAsync(expression);

            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    Context.Entry(item).State = EntityState.Deleted;
                    DbSet().Remove(item);
                }
            }
        }

        public async Task InsertOrUpdateAsync(TEntity entity)
        {
            var contextEntity = await GetByIdAsync(entity.Id);

            if (contextEntity == null)
                await DbSet().AddAsync(entity);
            else
            {
                Context.Entry(contextEntity).CurrentValues.SetValues(entity);
                Context.Entry(contextEntity).State = EntityState.Modified;
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).CurrentValues.SetValues(entity);
            Context.Entry(entity).State = EntityState.Modified;

            await Task.CompletedTask;
        }

        public async Task InsertRangeAsync(IList<TEntity> entities)
        {
            await DbSet().AddRangeAsync(entities);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbSet().AddAsync(entity);
        }

        public async Task<bool> AnyAsync()
        {
            return await DbSet().AnyAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || Context == null) return;

            Context.Dispose();
            Context = null;
        }

        #endregion
    }
}
