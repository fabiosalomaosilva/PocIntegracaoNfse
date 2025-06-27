using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Valor do serviço
/// </summary>
public class ValorServicoModel
{
    /// <summary>Valor recebido pelo intermediário</summary>
    public decimal? VReceb { get; set; }

    /// <summary>Valor dos serviços</summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal VServ { get; set; }
}