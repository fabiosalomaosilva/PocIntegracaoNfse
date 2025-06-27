using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Dados do prestador de serviços
/// Baseado no tipo complexo TCInfoPrestador
/// </summary>
public class PrestadorModel : PessoaBaseModel
{
    /// <summary>Regimes de tributação</summary>
    [Required]
    public RegimeTributarioModel RegTrib { get; set; } = new();
}