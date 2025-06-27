using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Tributação municipal (ISSQN)
/// </summary>
public class TributacaoMunicipalModel
{
    /// <summary>Tipo de tributação ISSQN</summary>
    [Required]
    public int TribISSQN { get; set; } = 1; // Temporário como int

    /// <summary>Alíquota</summary>
    public decimal? PAliq { get; set; }

    /// <summary>Tipo de retenção</summary>
    [Required]
    public int TpRetISSQN { get; set; } = 1; // Temporário como int
}