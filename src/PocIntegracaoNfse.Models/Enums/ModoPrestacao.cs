namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Modo de Prestação
/// Baseado em TSModoPrestacao
/// </summary>
public enum ModoPrestacao
{
    /// <summary>0 - Desconhecido (tipo não informado na nota de origem)</summary>
    Desconhecido = 0,

    /// <summary>1 - Transfronteiriço</summary>
    Transfronteirico = 1,

    /// <summary>2 - Consumo no Brasil</summary>
    ConsumoNoBrasil = 2,

    /// <summary>3 - Presença Comercial no Exterior</summary>
    PresencaComercialExterior = 3,

    /// <summary>4 - Movimento Temporário de Pessoas Físicas</summary>
    MovimentoTemporarioPessoas = 4
}