using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.TechChallenge.Infrastructure.Mappings;

[ExcludeFromCodeCoverage]
public class OrderMap : EntityTypeConfigurationBase<Domain.Entities.Order>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.Property(x => x.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(x => x.Number)
            .HasColumnName("number")
            .IsRequired();

        builder.Property(x => x.Total)
            .HasColumnName("total")
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder.Property(x => x.Situation)
            .HasColumnName("situation")
            .HasMaxLength(50)
            .IsRequired();

        builder.Ignore(x => x.OrderSituationEnum);

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("orders", "order");

        base.Configure(builder);
    }
}