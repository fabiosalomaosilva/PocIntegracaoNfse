using System.ComponentModel.DataAnnotations;
using PocIntegracaoNfse.Models.DPS;
using PocIntegracaoNfse.Models.Enums;

namespace PocIntegracaoNfse.Models.NFSe;

/// <summary>
/// Informações da NFSe
/// Baseado no tipo complexo TCInfNFSe
/// </summary>
public class InfNfseModel
{
    /// <summary>Identificador único da NFSe</summary>
    [Required]
    [StringLength(53)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Descrição do município emissor da NFSe</summary>
    [Required]
    [StringLength(150)]
    public string XLocEmi { get; set; } = string.Empty;

    /// <summary>Descrição do local da prestação do serviço</summary>
    [Required]
    [StringLength(150)]
    public string XLocPrestacao { get; set; } = string.Empty;

    /// <summary>Número sequencial da NFSe</summary>
    [Required]
    [StringLength(13)]
    public string NNFSe { get; set; } = string.Empty;

    /// <summary>Código do município de incidência do ISSQN (IBGE)</summary>
    [StringLength(7)]
    public string? CLocIncid { get; set; }

    /// <summary>Descrição do município de incidência do ISSQN</summary>
    [StringLength(150)]
    public string? XLocIncid { get; set; }

    /// <summary>Descrição da tributação nacional</summary>
    [StringLength(600)]
    public string? XTribNac { get; set; }

    /// <summary>Descrição da tributação municipal</summary>
    [StringLength(600)]
    public string? XTribMun { get; set; }

    /// <summary>Descrição do código NBS</summary>
    [StringLength(600)]
    public string? XNBS { get; set; }

    /// <summary>Versão do aplicativo que gerou a NFSe</summary>
    [Required]
    [StringLength(20)]
    public string VerAplic { get; set; } = "1.00";

    /// <summary>Ambiente gerador da NFSe</summary>
    [Required]
    public AmbienteGeradorNFSe AmbGer { get; set; } = AmbienteGeradorNFSe.SistemaNacional;

    /// <summary>Tipo de emissão da NFSe</summary>
    [Required]
    public TipoEmissao TpEmis { get; set; } = TipoEmissao.EmissaoNormalNacional;

    /// <summary>Processo de emissão da DPS</summary>
    [Required]
    public ProcessoEmissao ProcEmi { get; set; } = ProcessoEmissao.AplicativoContribuinte;

    /// <summary>Código do status da mensagem</summary>
    [Required]
    public int CStat { get; set; } = 100;

    /// <summary>Data/hora da validação da DPS e geração da NFSe</summary>
    [Required]
    public DateTime DhProc { get; set; } = DateTime.Now;

    /// <summary>Número sequencial do documento gerado por ambiente gerador</summary>
    [Required]
    [StringLength(13)]
    public string NDFSe { get; set; } = string.Empty;

    /// <summary>Dados do emitente da NFSe</summary>
    [Required]
    public EmitenteNfseModel Emit { get; set; } = new();

    /// <summary>Valores da NFSe</summary>
    [Required]
    public ValoresNfseModel Valores { get; set; } = new();

    /// <summary>DPS que originou esta NFSe</summary>
    [Required]
    public DpsModel DPS { get; set; } = new();
}