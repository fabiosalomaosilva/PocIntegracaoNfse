namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Processo de Emissão da DPS
/// Baseado em TSProcEmissao
/// </summary>
public enum ProcessoEmissao
{
    /// <summary>1 - Emissão com aplicativo do contribuinte (via Web Service)</summary>
    AplicativoContribuinte = 1,

    /// <summary>2 - Emissão com aplicativo disponibilizado pelo fisco (Web)</summary>
    AplicativoFiscoWeb = 2,

    /// <summary>3 - Emissão com aplicativo disponibilizado pelo fisco (App)</summary>
    AplicativoFiscoApp = 3
}