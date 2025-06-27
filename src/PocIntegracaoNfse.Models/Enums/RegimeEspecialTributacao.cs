namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Tipos de Regimes Especiais de Tributação
/// Baseado em TSRegEspTrib
/// </summary>
public enum RegimeEspecialTributacao
{
    /// <summary>0 - Nenhum</summary>
    Nenhum = 0,

    /// <summary>1 - Ato Cooperado (Cooperativa)</summary>
    AtoCooperado = 1,

    /// <summary>2 - Estimativa</summary>
    Estimativa = 2,

    /// <summary>3 - Microempresa Municipal</summary>
    MicroempresaMunicipal = 3,

    /// <summary>4 - Notário ou Registrador</summary>
    NotarioRegistrador = 4,

    /// <summary>5 - Profissional Autônomo</summary>
    ProfissionalAutonomo = 5,

    /// <summary>6 - Sociedade de Profissionais</summary>
    SociedadeProfissionais = 6
}