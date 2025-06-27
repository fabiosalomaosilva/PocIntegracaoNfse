using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Local da prestação do serviço
/// </summary>
public class LocalPrestacaoModel
{
    /// <summary>Código do município onde o serviço foi prestado</summary>
    [StringLength(7, MinimumLength = 7)]
    public string? CLocPrestacao { get; set; }

    /// <summary>Código do país onde o serviço foi prestado</summary>
    [StringLength(2, MinimumLength = 2)]
    public string? CPaisPrestacao { get; set; }
}