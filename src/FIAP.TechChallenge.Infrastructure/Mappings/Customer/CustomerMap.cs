using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Infrastructure.Mappings;
using FIAP.TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.TechChallenge.Infrastructure.Mappings;

[ExcludeFromCodeCoverage]
public class CustomerMap : EntityTypeConfigurationBase<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(150);

        builder.Property(x => x.Document)
            .HasColumnName("document")
            .HasMaxLength(11);

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(250);

        builder.ToTable("customers", "customer");

        base.Configure(builder);
    }
}