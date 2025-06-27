using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Código do serviço prestado
/// </summary>
public class CodigoServicoModel
{
    /// <summary>Código de tributação nacional (6 dígitos)</summary>
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string CTribNac { get; set; } = string.Empty;

    /// <summary>Código de tributação municipal</summary>
    [StringLength(3, MinimumLength = 3)]
    public string? CTribMun { get; set; }

    /// <summary>Descrição do serviço</summary>
    [Required]
    [StringLength(2000)]
    public string XDescServ { get; set; } = string.Empty;

    /// <summary>Código NBS</summary>
    [StringLength(9, MinimumLength = 9)]
    public string? CNBS { get; set; }

    /// <summary>Código interno do contribuinte</summary>
    [StringLength(20)]
    public string? CIntContrib { get; set; }
}