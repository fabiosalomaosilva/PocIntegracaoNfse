using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Declaração de Prestação de Serviços - DPS
/// Baseado no tipo complexo TCDPS
/// </summary>
public class DpsModel
{
    /// <summary>Versão do schema (sempre "1.00")</summary>
    public string Versao { get; set; } = "1.00";

    /// <summary>Informações da DPS</summary>
    [Required]
    public InfDpsModel InfDPS { get; set; } = new();

    /// <summary>Assinatura digital (opcional por enquanto)</summary>
    public object? Signature { get; set; }
}