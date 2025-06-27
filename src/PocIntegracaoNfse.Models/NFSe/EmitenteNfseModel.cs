using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.NFSe;

/// <summary>
/// Dados do emitente da NFSe
/// Baseado no tipo complexo TCEmitente
/// </summary>
public class EmitenteNfseModel
{
    /// <summary>Número do CNPJ (exclusivo com CPF)</summary>
    [StringLength(14)]
    public string? CNPJ { get; set; }

    /// <summary>Número do CPF (exclusivo com CNPJ)</summary>
    [StringLength(11)]
    public string? CPF { get; set; }

    /// <summary>Número da inscrição municipal</summary>
    [StringLength(15)]
    public string? IM { get; set; }

    /// <summary>Nome/Razão Social do emitente</summary>
    [Required]
    [StringLength(300)]
    public string XNome { get; set; } = string.Empty;

    /// <summary>Nome fantasia do emitente</summary>
    [StringLength(60)]
    public string? XFant { get; set; }

    /// <summary>Endereço nacional do emitente</summary>
    [Required]
    public EnderecoEmitenteModel EnderNac { get; set; } = new();

    /// <summary>Telefone do emitente</summary>
    [StringLength(20)]
    public string? Fone { get; set; }

    /// <summary>E-mail do emitente</summary>
    [StringLength(60)]
    public string? Email { get; set; }
}