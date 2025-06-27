namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Tipo de retenção do ISSQN
/// Baseado em TSTipoRetISSQN
/// </summary>
public enum TipoRetencaoISSQN
{
    /// <summary>1 - Não Retido</summary>
    NaoRetido = 1,

    /// <summary>2 - Retido pelo Tomador</summary>
    RetidoTomador = 2,

    /// <summary>3 - Retido pelo Intermediário</summary>
    RetidoIntermediario = 3
}