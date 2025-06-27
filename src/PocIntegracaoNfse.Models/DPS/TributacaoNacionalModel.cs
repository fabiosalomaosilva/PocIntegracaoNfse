namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Tributação nacional (PIS/COFINS)
/// </summary>
public class TributacaoNacionalModel
{
    /// <summary>Valor retenção CP</summary>
    public decimal? VRetCP { get; set; }

    /// <summary>Valor retenção IRRF</summary>
    public decimal? VRetIRRF { get; set; }

    /// <summary>Valor retenção CSLL</summary>
    public decimal? VRetCSLL { get; set; }
}