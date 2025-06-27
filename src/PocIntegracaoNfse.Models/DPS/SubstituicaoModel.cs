using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Dados da NFS-e a ser substituída
/// </summary>
public class SubstituicaoModel
{
    /// <summary>Chave de acesso da NFS-e a ser substituída</summary>
    [Required]
    [StringLength(50, MinimumLength = 50)]
    public string ChSubstda { get; set; } = string.Empty;

    /// <summary>Código de justificativa para substituição</summary>
    [Required]
    public string CMotivo { get; set; } = string.Empty;

    /// <summary>Descrição do motivo (opcional)</summary>
    [StringLength(255, MinimumLength = 15)]
    public string? XMotivo { get; set; }
}