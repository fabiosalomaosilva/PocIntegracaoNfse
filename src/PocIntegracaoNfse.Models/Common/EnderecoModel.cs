using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Endereço completo (nacional ou exterior)
/// </summary>
public class EnderecoModel
{
    /// <summary>Dados específicos do endereço nacional</summary>
    public EnderecoNacionalModel? EndNac { get; set; }

    /// <summary>Dados específicos do endereço no exterior</summary>
    public EnderecoExteriorModel? EndExt { get; set; }

    /// <summary>Logradouro</summary>
    [Required]
    [StringLength(255)]
    public string XLgr { get; set; } = string.Empty;

    /// <summary>Número</summary>
    [Required]
    [StringLength(60)]
    public string Nro { get; set; } = string.Empty;

    /// <summary>Complemento</summary>
    [StringLength(156)]
    public string? XCpl { get; set; }

    /// <summary>Bairro</summary>
    [Required]
    [StringLength(60)]
    public string XBairro { get; set; } = string.Empty;
}