using PocIntegracaoNfse.Models.Common;
using PocIntegracaoNfse.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Informações da DPS
/// Baseado no tipo complexo TCInfDPS
/// </summary>
public class InfDpsModel
{
    /// <summary>Identificador único da DPS</summary>
    [Required]
    public string Id { get; set; } = string.Empty;

    /// <summary>Tipo de ambiente</summary>
    [Required]
    public TipoAmbiente TpAmb { get; set; } = TipoAmbiente.Homologacao;

    /// <summary>Data e hora de emissão</summary>
    [Required]
    public DateTime DhEmi { get; set; } = DateTime.Now;

    /// <summary>Versão do aplicativo</summary>
    [Required]
    [StringLength(20)]
    public string VerAplic { get; set; } = "1.00";

    /// <summary>Série da DPS</summary>
    [Required]
    [StringLength(5)]
    public string Serie { get; set; } = "00001";

    /// <summary>Número da DPS</summary>
    [Required]
    [StringLength(15)]
    public string NDPS { get; set; } = "1";

    /// <summary>Data de competência</summary>
    [Required]
    public DateOnly DCompet { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>Tipo de emitente</summary>
    [Required]
    public EmitenteDPS TpEmit { get; set; } = EmitenteDPS.Prestador;

    /// <summary>Código do município emissor (IBGE)</summary>
    [Required]
    [StringLength(7, MinimumLength = 7)]
    public string CLocEmi { get; set; } = string.Empty;

    /// <summary>Dados de substituição (opcional)</summary>
    public SubstituicaoModel? Subst { get; set; }

    /// <summary>Dados do prestador</summary>
    [Required]
    public PrestadorModel Prest { get; set; } = new();

    /// <summary>Dados do tomador (opcional)</summary>
    public TomadorModel? Toma { get; set; }

    /// <summary>Dados do intermediário (opcional)</summary>
    public IntermediarioModel? Interm { get; set; }

    /// <summary>Dados do serviço</summary>
    [Required]
    public ServicoModel Serv { get; set; } = new();

    /// <summary>Valores do serviço</summary>
    [Required]
    public ValoresModel Valores { get; set; } = new();
}