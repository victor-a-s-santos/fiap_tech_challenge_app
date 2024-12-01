using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Infrastructure.Mappings;
using FIAP.TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.TechChallenge.Infrastructure.Mappings.Order;

[ExcludeFromCodeCoverage]
public class OrderItemMap : EntityTypeConfigurationBase<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("items", "order");

        base.Configure(builder);
    }
}