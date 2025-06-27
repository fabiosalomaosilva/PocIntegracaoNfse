namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Tributação do ISSQN sobre o serviço prestado
/// Baseado em TSTribISSQN
/// </summary>
public enum TributacaoISSQN
{
    /// <summary>1 - Operação tributável</summary>
    OperacaoTributavel = 1,

    /// <summary>2 - Exportação de serviço</summary>
    ExportacaoServico = 2,

    /// <summary>3 - Não Incidência</summary>
    NaoIncidencia = 3,

    /// <summary>4 - Imunidade</summary>
    Imunidade = 4
}