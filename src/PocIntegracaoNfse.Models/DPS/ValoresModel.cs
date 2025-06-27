using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Valores do serviço prestado
/// </summary>
public class ValoresModel
{
    /// <summary>Valores do serviço</summary>
    [Required]
    public ValorServicoModel VServPrest { get; set; } = new();

    /// <summary>Descontos</summary>
    public DescontosModel? VDescCondIncond { get; set; }

    /// <summary>Deduções/Reduções</summary>
    public DeducoesModel? VDedRed { get; set; }

    /// <summary>Tributação</summary>
    [Required]
    public TributacaoModel Trib { get; set; } = new();
}