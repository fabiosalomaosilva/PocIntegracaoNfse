using System.ComponentModel.DataAnnotations;
using PocIntegracaoNfse.Models.Enums;

namespace PocIntegracaoNfse.Models.Common;

/// <summary>
/// Base para pessoas envolvidas na NFS-e
/// </summary>
public abstract class PessoaBaseModel
{
    /// <summary>CNPJ (se pessoa jurídica)</summary>
    [StringLength(14, MinimumLength = 14)]
    public string? CNPJ { get; set; }

    /// <summary>CPF (se pessoa física)</summary>
    [StringLength(11, MinimumLength = 11)]
    public string? CPF { get; set; }

    /// <summary>NIF (se estrangeiro)</summary>
    [StringLength(40)]
    public string? NIF { get; set; }

    /// <summary>Motivo para não informar NIF</summary>
    public MotivoNaoNIF? CNaoNIF { get; set; }

    /// <summary>CAEPF</summary>
    [StringLength(14, MinimumLength = 14)]
    public string? CAEPF { get; set; }

    /// <summary>Inscrição Municipal</summary>
    [StringLength(15)]
    public string? IM { get; set; }

    /// <summary>Nome/Razão Social</summary>
    [StringLength(300)]
    public string? XNome { get; set; }

    /// <summary>Endereço</summary>
    public EnderecoModel? End { get; set; }

    /// <summary>Telefone</summary>
    [StringLength(20, MinimumLength = 6)]
    public string? Fone { get; set; }

    /// <summary>E-mail</summary>
    [StringLength(80)]
    [EmailAddress]
    public string? Email { get; set; }
}