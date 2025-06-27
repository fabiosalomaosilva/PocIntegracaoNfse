using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Dados específicos de endereço nacional
/// </summary>
public class EnderecoNacionalModel
{
    /// <summary>Código do município (IBGE)</summary>
    [Required]
    [StringLength(7, MinimumLength = 7)]
    public string CMun { get; set; } = string.Empty;

    /// <summary>CEP</summary>
    [Required]
    [StringLength(8, MinimumLength = 8)]
    public string CEP { get; set; } = string.Empty;
}