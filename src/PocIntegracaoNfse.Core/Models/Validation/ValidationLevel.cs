namespace PocIntegracaoNfse.Core.Models.Validation;

/// <summary>
/// Níveis de validação
/// </summary>
public enum ValidationLevel
{
    /// <summary>Validação básica - apenas campos obrigatórios</summary>
    Basic,

    /// <summary>Validação flexível - aceita incompatibilidades conhecidas</summary>
    Flexible,

    /// <summary>Validação recomendada - avisos para melhorias</summary>
    Recommended,

    /// <summary>Validação estrita - 100% conforme XSD</summary>
    Strict
}