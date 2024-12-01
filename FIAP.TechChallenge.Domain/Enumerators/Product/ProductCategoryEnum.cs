using System.ComponentModel;

namespace FIAP.TechChallenge.Domain.Enumerators;

public enum ProductCategoryEnum
{
    [Description("Lanche")]
    Sandwich,

    [Description("Acompanhamento")]
    Accompaniment,

    [Description("Bebida")]
    Beverage,

    [Description("Sobremesa")]
    Dessert
}