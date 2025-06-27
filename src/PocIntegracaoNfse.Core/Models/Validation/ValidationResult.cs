namespace PocIntegracaoNfse.Core.Models.Validation;

/// <summary>
/// Resultado da validação de XML
/// </summary>
public class ValidationResult
{
    /// <summary>Construtor padrão</summary>
    public ValidationResult()
    {

    }

    /// <summary>Indica se a validação passou</summary>
    public bool IsValid { get; set; }

    /// <summary>Lista de erros (impedem processamento)</summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>Lista de avisos (não impedem processamento)</summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>Nível de validação aplicado</summary>
    public ValidationLevel ValidationLevel { get; set; }

    /// <summary>Tempo de execução da validação</summary>
    public TimeSpan ElapsedTime { get; set; }

    /// <summary>Detalhes adicionais da validação</summary>
    public Dictionary<string, object> Details { get; set; } = new();
}