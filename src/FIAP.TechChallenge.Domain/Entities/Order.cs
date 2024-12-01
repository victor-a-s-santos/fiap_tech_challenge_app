using System.Diagnostics.CodeAnalysis;
using FIAP.Crosscutting.Domain.Entities;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.TechChallenge.Domain.Enumerators;

namespace FIAP.TechChallenge.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Order : Entity
{
    public Guid? CustomerId { get; set; }
    public Int64 Number { get; set; }
    public decimal Total { get; set; }
    public string Situation { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public OrderSituationEnum OrderSituationEnum
    {
        get { return EnumExtension.GetEnumerator<OrderSituationEnum>(Situation?.Trim()); }
        set { Situation = value.GetDescription(); }
    }
}