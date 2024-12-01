using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Entities;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.TechChallenge.Domain.Enumerators;

namespace FIAP.TechChallenge.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }

    public ProductCategoryEnum ProductCategoryEnum
    {
        get { return EnumExtension.GetEnumerator<ProductCategoryEnum>(Category?.Trim()); }
        set { Category = value.GetDescription(); }
    }
}