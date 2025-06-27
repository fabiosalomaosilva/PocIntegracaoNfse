namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Total de tributos
/// </summary>
public class TotalTributosModel
{
    /// <summary>Valor total tributos federais</summary>
    public decimal VTotTribFed { get; set; }

    /// <summary>Valor total tributos estaduais</summary>
    public decimal VTotTribEst { get; set; }

    /// <summary>Valor total tributos municipais</summary>
    public decimal VTotTribMun { get; set; }
}