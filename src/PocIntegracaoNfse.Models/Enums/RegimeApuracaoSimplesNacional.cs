namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Regime de apuração para optantes do Simples Nacional ME/EPP
/// Baseado em TSRegimeApuracaoSimpNac
/// </summary>
public enum RegimeApuracaoSimplesNacional
{
    /// <summary>1 – Regime de apuração dos tributos federais e municipal pelo SN</summary>
    TributosFederaisEMunicipalSN = 1,

    /// <summary>2 – Regime de apuração dos tributos federais pelo SN e ISSQN por fora do SN conforme respectiva legislação municipal do tributo</summary>
    TributosFederaisSNISSQNForaSN = 2,

    /// <summary>3 – Regime de apuração dos tributos federais e municipal por fora do SN conforme respectivas legislações federal e municipal de cada tributo</summary>
    TributosFederaisEMunicipalForaSN = 3
}