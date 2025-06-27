using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.NFSe;

/// <summary>
/// Nota Fiscal de Serviços Eletrônica - NFSe
/// Baseado no tipo complexo TCNFSe
/// </summary>
public class NfseModel
{
    /// <summary>Versão do schema (sempre "1.00")</summary>
    public string Versao { get; set; } = "1.00";

    /// <summary>Informações da NFSe</summary>
    [Required]
    public InfNfseModel InfNFSe { get; set; } = new();

    /// <summary>Assinatura digital</summary>
    public object? Signature { get; set; }
}