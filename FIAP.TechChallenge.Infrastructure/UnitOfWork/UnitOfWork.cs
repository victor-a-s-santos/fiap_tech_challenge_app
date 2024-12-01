using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using FIAP.TechChallenge.Infrastructure.Contexts;

namespace FIAP.TechChallenge.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlContext _context;

        public UnitOfWork(SqlContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            using var transaction = _context.Database.BeginTransaction();
            bool success = (await _context.SaveChangesAsync()) > 0;

            if (success)
                await transaction.CommitAsync();
            else
                await transaction.RollbackAsync();

            return success;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
