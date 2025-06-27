using System.ComponentModel.DataAnnotations;
using PocIntegracaoNfse.Models.Enums;

namespace PocIntegracaoNfse.Models.NFSe;

/// <summary>
/// Endereço do emitente da NFSe
/// Baseado no tipo complexo TCEnderecoEmitente
/// </summary>
public class EnderecoEmitenteModel
{
    /// <summary>Logradouro</summary>
    [Required]
    [StringLength(200)]
    public string XLgr { get; set; } = string.Empty;

    /// <summary>Número</summary>
    [Required]
    [StringLength(10)]
    public string Nro { get; set; } = string.Empty;

    /// <summary>Complemento</summary>
    [StringLength(60)]
    public string? XCpl { get; set; }

    /// <summary>Bairro</summary>
    [Required]
    [StringLength(60)]
    public string XBairro { get; set; } = string.Empty;

    /// <summary>Código do município (IBGE)</summary>
    [Required]
    [StringLength(7)]
    public string CMun { get; set; } = string.Empty;

    /// <summary>Sigla da UF</summary>
    [Required]
    public UnidadeFederacao UF { get; set; }

    /// <summary>CEP</summary>
    [Required]
    [StringLength(8)]
    public string CEP { get; set; } = string.Empty;
}