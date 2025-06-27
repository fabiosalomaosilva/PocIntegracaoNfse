using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Dados específicos de endereço no exterior
/// </summary>
public class EnderecoExteriorModel
{
    /// <summary>Código do país (ISO)</summary>
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string CPais { get; set; } = string.Empty;

    /// <summary>Código de endereçamento postal</summary>
    [Required]
    [StringLength(11)]
    public string CEndPost { get; set; } = string.Empty;

    /// <summary>Cidade</summary>
    [Required]
    [StringLength(60)]
    public string XCidade { get; set; } = string.Empty;

    /// <summary>Estado/Província/Região</summary>
    [Required]
    [StringLength(60)]
    public string XEstProvReg { get; set; } = string.Empty;
}