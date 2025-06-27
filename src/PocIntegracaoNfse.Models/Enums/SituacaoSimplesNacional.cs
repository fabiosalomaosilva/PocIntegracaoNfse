namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Situação perante o Simples Nacional
/// Baseado em TSOpSimpNac
/// </summary>
public enum SituacaoSimplesNacional
{
    /// <summary>1 - Não Optante</summary>
    NaoOptante = 1,

    /// <summary>2 - Optante - Microempreendedor Individual (MEI)</summary>
    OptanteMEI = 2,

    /// <summary>3 - Optante - Microempresa ou Empresa de Pequeno Porte (ME/EPP)</summary>
    OptanteMEEPP = 3
}