using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Infrastructure.Mappings;
using FIAP.TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.TechChallenge.Infrastructure.Mappings;

[ExcludeFromCodeCoverage]
public class ProductMap : EntityTypeConfigurationBase<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Ignore(x => x.ProductCategoryEnum);

        builder.ToTable("products", "product");

        base.Configure(builder);
    }
}