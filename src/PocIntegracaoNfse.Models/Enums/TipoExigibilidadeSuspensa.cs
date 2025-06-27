namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Opção para Exigibilidade Suspensa
/// Baseado em TSOpExigSuspensa
/// </summary>
public enum TipoExigibilidadeSuspensa
{
    /// <summary>1 - Exigibilidade Suspensa por Decisão Judicial</summary>
    DecisaoJudicial = 1,

    /// <summary>2 - Exigibilidade Suspensa por Processo Administrativo</summary>
    ProcessoAdministrativo = 2
}