namespace PocIntegracaoNfse.Models.Constants;

/// <summary>
/// Problemas de compatibilidade conhecidos entre XML base e schemas XSD
/// </summary>
public static class KnownCompatibilityIssues
{
    /// <summary>
    /// Mapeamento de campos com nomes diferentes
    /// </summary>
    public static readonly Dictionary<string, string> FieldMappings = new()
    {
        ["tribFed"] = "tribNac",
        ["tpEmis"] = "procEmi",
        ["nSeqEvento"] = "nPedRegEvento"
    };

    /// <summary>
    /// Campos que existem no XML base mas não nos XSDs
    /// </summary>
    public static readonly HashSet<string> ExtraFieldsInXml = new()
    {
        "tribFed.piscofins",
        "tribFed.vRetCP",
        "tribFed.vRetIRRF",
        "tribFed.vRetCSLL"
    };

    /// <summary>
    /// Validações que devem ser ignoradas por incompatibilidade conhecida
    /// </summary>
    public static readonly HashSet<string> IgnoredValidations = new()
    {
        "tribFed.structure.mismatch",
        "procedimento.emissao.name.difference",
        "tributos.federais.vs.nacionais",
        "processo.emissao.vs.tipo.emissao"
    };

    /// <summary>
    /// Estruturas que diferem entre XML base e XSD
    /// </summary>
    public static readonly Dictionary<string, string> StructureDifferences = new()
    {
        ["valores.tribFed"] = "Estrutura 'tribFed' no XML base deve ser mapeada para 'tribNac' no XSD",
        ["identificacao.tpEmis"] = "Campo 'tpEmis' no XML base corresponde a 'procEmi' no XSD",
        ["totais.tributarios"] = "Estrutura de totais pode variar entre implementações municipais"
    };

    /// <summary>
    /// Verifica se um campo deve ser ignorado na validação
    /// </summary>
    public static bool ShouldIgnoreValidation(string fieldPath)
    {
        return IgnoredValidations.Contains(fieldPath) ||
               ExtraFieldsInXml.Contains(fieldPath);
    }

    /// <summary>
    /// Obtém o mapeamento correto para um campo
    /// </summary>
    public static string GetMappedField(string originalField)
    {
        return FieldMappings.TryGetValue(originalField, out var mapped) ? mapped : originalField;
    }
}