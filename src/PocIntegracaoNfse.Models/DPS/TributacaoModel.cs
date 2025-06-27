using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Tributação completa
/// </summary>
public class TributacaoModel
{
    /// <summary>Tributação municipal</summary>
    [Required]
    public TributacaoMunicipalModel TribMun { get; set; } = new();

    /// <summary>Tributação nacional</summary>
    public TributacaoNacionalModel? TribNac { get; set; }

    /// <summary>Total de tributos</summary>
    [Required]
    public TotalTributosModel TotTrib { get; set; } = new();
}