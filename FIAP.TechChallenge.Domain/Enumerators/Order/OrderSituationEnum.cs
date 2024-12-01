using System.ComponentModel;

namespace FIAP.TechChallenge.Domain.Enumerators;

public enum OrderSituationEnum
{
    [Description("Recebido")]
    Received,

    [Description("Em preparação")]
    InPreparation,

    [Description("Pronto")]
    Ready,

    [Description("Finalizado")]
    Completed
}