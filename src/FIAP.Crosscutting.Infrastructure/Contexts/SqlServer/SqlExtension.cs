using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.Crosscutting.Infrastructure.Contexts.SqlServer
{
    public static class SqlExtension
    {
        public static void AddSqlContext<TContext>(
           this IServiceCollection services,
           IConfiguration configuration) where TContext : DbContext
        {
            services.AddDbContext<TContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            services.AddScoped<DbContext, TContext>();
        }

        public static void AddPostgressContext<TContext>(
           this IServiceCollection services,
           IConfiguration configuration) where TContext : DbContext
        {
            services.AddDbContext<TContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("SqlServer"));
            });

            services.AddScoped<DbContext, TContext>();
        }
    }
}
