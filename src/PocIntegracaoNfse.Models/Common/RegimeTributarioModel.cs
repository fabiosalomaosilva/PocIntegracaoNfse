using PocIntegracaoNfse.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Regimes de tributação do prestador
/// </summary>
public class RegimeTributarioModel
{
    /// <summary>Situação perante o Simples Nacional</summary>
    [Required]
    public SituacaoSimplesNacional OpSimpNac { get; set; } = SituacaoSimplesNacional.NaoOptante;

    /// <summary>Regime de apuração (só para ME/EPP)</summary>
    public RegimeApuracaoSimplesNacional? RegApTribSN { get; set; }

    /// <summary>Regime especial de tributação</summary>
    [Required]
    public RegimeEspecialTributacao RegEspTrib { get; set; } = RegimeEspecialTributacao.Nenhum;
}