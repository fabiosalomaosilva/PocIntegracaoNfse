namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Descontos condicionados e incondicionados
/// </summary>
public class DescontosModel
{
    /// <summary>Desconto incondicionado</summary>
    public decimal? VDescIncond { get; set; }

    /// <summary>Desconto condicionado</summary>
    public decimal? VDescCond { get; set; }
}