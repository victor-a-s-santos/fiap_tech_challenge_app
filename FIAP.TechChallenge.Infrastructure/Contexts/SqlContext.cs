using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Infrastructure.Mappings;
using FIAP.TechChallenge.Infrastructure.Mappings.Order;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChallenge.Infrastructure.Contexts;

[ExcludeFromCodeCoverage]
public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options)
        : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new ProductMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.ApplyConfiguration(new OrderItemMap());

        base.OnModelCreating(modelBuilder);
    }
}