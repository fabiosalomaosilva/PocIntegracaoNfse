using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Informações complementares
/// </summary>
public class InformacoesComplementaresModel
{
    /// <summary>Identificador de documento técnico</summary>
    [StringLength(40)]
    public string? IdDocTec { get; set; }

    /// <summary>Documento de referência</summary>
    [StringLength(255)]
    public string? DocRef { get; set; }

    /// <summary>Informações complementares</summary>
    [StringLength(2000)]
    public string? XInfComp { get; set; }
}