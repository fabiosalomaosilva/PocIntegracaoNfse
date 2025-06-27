namespace PocIntegracaoNfse.Models.Constants;

/// <summary>
/// Padrões de identificação de documentos
/// </summary>
public static class DocumentPatterns
{
    /// <summary>Padrão regex para ID da DPS</summary>
    public const string DPSIdPattern = @"DPS[0-9]{42}";

    /// <summary>Padrão regex para ID da NFSe</summary>
    public const string NFSeIdPattern = @"NFS[0-9]{50}";

    /// <summary>Padrão regex para chave da NFSe</summary>
    public const string ChaveNFSePattern = @"[0-9]{50}";

    /// <summary>Padrão regex para CNPJ</summary>
    public const string CNPJPattern = @"[0-9]{14}";

    /// <summary>Padrão regex para CPF</summary>
    public const string CPFPattern = @"[0-9]{11}";

    /// <summary>Padrão regex para código municipal IBGE</summary>
    public const string CodigoMunicipalPattern = @"[0-9]{7}";

    /// <summary>Padrão regex para CEP</summary>
    public const string CEPPattern = @"[0-9]{8}";
}