namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Código de Situação Tributária do PIS/COFINS (CST)
/// Baseado em TSTipoCST
/// </summary>
public enum CodigoSituacaoTributariaPISCOFINS
{
    /// <summary>00 - Nenhum</summary>
    Nenhum = 0,

    /// <summary>01 - Operação Tributável com Alíquota Básica</summary>
    TributavelAliquotaBasica = 1,

    /// <summary>02 - Operação Tributável com Alíquota Diferenciada</summary>
    TributavelAliquotaDiferenciada = 2,

    /// <summary>03 - Operação Tributável com Alíquota por Unidade de Medida de Produto</summary>
    TributavelAliquotaUnidadeMedida = 3,

    /// <summary>04 - Operação Tributável monofásica - Revenda a Alíquota Zero</summary>
    TributavelMonofasicaAliquotaZero = 4,

    /// <summary>05 - Operação Tributável por Substituição Tributária</summary>
    TributavelSubstituicaoTributaria = 5,

    /// <summary>06 - Operação Tributável a Alíquota Zero</summary>
    TributavelAliquotaZero = 6,

    /// <summary>07 - Operação Tributável da Contribuição</summary>
    TributavelContribuicao = 7,

    /// <summary>08 - Operação sem Incidência da Contribuição</summary>
    SemIncidenciaContribuicao = 8,

    /// <summary>09 - Operação com Suspensão da Contribuição</summary>
    SuspensaoContribuicao = 9
}