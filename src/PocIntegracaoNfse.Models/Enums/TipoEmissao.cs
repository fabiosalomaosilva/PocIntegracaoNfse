namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Tipo de emissão da NFS-e
/// Baseado em TSTipoEmissao
/// </summary>
public enum TipoEmissao
{
    /// <summary>1 - Emissão normal no modelo da NFS-e Nacional</summary>
    EmissaoNormalNacional = 1,

    /// <summary>2 - Emissão original em leiaute próprio do município com transcrição para o modelo da NFS-e Nacional</summary>
    EmissaoOriginalMunicipal = 2
}