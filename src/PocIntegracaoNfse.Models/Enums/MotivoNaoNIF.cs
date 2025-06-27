namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Motivo para não informação do NIF
/// Baseado em TSCodNaoNIF
/// </summary>
public enum MotivoNaoNIF
{
    /// <summary>0 - Não informado na nota de origem</summary>
    NaoInformadoOrigem = 0,

    /// <summary>1 - Dispensado do NIF</summary>
    DispensadoNIF = 1,

    /// <summary>2 - Não exigência do NIF</summary>
    NaoExigenciaNIF = 2
}