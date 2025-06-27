using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.NFSe;

/// <summary>
/// Valores da NFSe
/// Baseado no tipo complexo TCValoresNFSe
/// </summary>
public class ValoresNfseModel
{
    /// <summary>Valor monetário de dedução/redução da base de cálculo do ISSQN</summary>
    public decimal? VCalcDR { get; set; }

    /// <summary>Tipo de Benefício Municipal</summary>
    [StringLength(40)]
    public string? TpBM { get; set; }

    /// <summary>Valor monetário do percentual de redução da BC devido a benefício municipal</summary>
    public decimal? VCalcBM { get; set; }

    /// <summary>Valor da base de cálculo do ISSQN</summary>
    public decimal? VBC { get; set; }

    /// <summary>Alíquota aplicada sobre a base de cálculo</summary>
    public decimal? PAliqAplic { get; set; }

    /// <summary>Valor do ISSQN</summary>
    public decimal? VISSQN { get; set; }

    /// <summary>Valor total de retenções</summary>
    public decimal? VTotalRet { get; set; }

    /// <summary>Valor líquido</summary>
    [Required]
    public decimal VLiq { get; set; }

    /// <summary>Informações adicionais de uso da Administração Tributária Municipal</summary>
    [StringLength(2000)]
    public string? XOutInf { get; set; }
}