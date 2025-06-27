using MudBlazor;
using PocIntegracaoNfse.Core.Services;
using PocIntegracaoNfse.Models.DPS;
using PocIntegracaoNfse.Models.NFSe;

namespace PocIntegracaoNfse.Services;

/// <summary>
/// Service para orquestração de operações XML (DPS e NFSe)
/// </summary>
public class XmlService
{
    private readonly XmlGeneratorService _xmlGenerator;
    private readonly XmlValidationService _xmlValidator;
    private readonly XmlParserService _xmlParser;
    private readonly ISnackbar _snackbar;

    public XmlService(
        XmlGeneratorService xmlGenerator,
        XmlValidationService xmlValidator,
        XmlParserService xmlParser,
        ISnackbar snackbar)
    {
        _xmlGenerator = xmlGenerator;
        _xmlValidator = xmlValidator;
        _xmlParser = xmlParser;
        _snackbar = snackbar;
    }

    #region DPS Operations

    /// <summary>
    /// Gera XML a partir de um model DPS
    /// </summary>
    public async Task<string> GenerateXmlAsync(DpsModel dps)
    {
        try
        {
            var xml = await _xmlGenerator.GenerateXmlAsync(dps);
            _snackbar.Add("XML DPS gerado com sucesso!", Severity.Success);
            return xml;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao gerar XML DPS: {ex.Message}", Severity.Error);
            throw;
        }
    }

    /// <summary>
    /// Faz parse de XML DPS para modelo
    /// </summary>
    public async Task<DpsModel?> ParseDpsXmlAsync(string xml)
    {
        try
        {
            var dps = await _xmlParser.ParseDpsAsync(xml);
            _snackbar.Add("XML DPS importado com sucesso!", Severity.Success);
            return dps;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao importar XML DPS: {ex.Message}", Severity.Error);
            return null;
        }
    }

    #endregion

    #region NFSe Operations

    /// <summary>
    /// Gera XML a partir de um model NFSe
    /// </summary>
    public async Task<string> GenerateNfseXmlAsync(NfseModel nfse)
    {
        try
        {
            var xml = await _xmlGenerator.GenerateNfseXmlAsync(nfse);
            _snackbar.Add("XML NFSe gerado com sucesso!", Severity.Success);
            return xml;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao gerar XML NFSe: {ex.Message}", Severity.Error);
            throw;
        }
    }

    /// <summary>
    /// Faz parse de XML NFSe para modelo
    /// TODO: Implementar quando necessário
    /// </summary>
    public async Task<NfseModel?> ParseNfseXmlAsync(string xml)
    {
        try
        {
            // TODO: Implementar ParseNfseAsync no XmlParserService
            await Task.Delay(100);
            _snackbar.Add("Parse NFSe ainda não implementado", Severity.Warning);
            return null;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao importar XML NFSe: {ex.Message}", Severity.Error);
            return null;
        }
    }

    #endregion

    #region Generic Operations

    /// <summary>
    /// Gera XML com detecção automática do tipo
    /// </summary>
    public async Task<string> GenerateXmlAsync(object model)
    {
        try
        {
            var xml = await _xmlGenerator.GenerateXmlAsync(model);
            var docType = _xmlGenerator.GetDocumentType(model);
            _snackbar.Add($"XML {docType} gerado com sucesso!", Severity.Success);
            return xml;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao gerar XML: {ex.Message}", Severity.Error);
            throw;
        }
    }

    /// <summary>
    /// Valida XML contra schemas
    /// </summary>
    public async Task<bool> ValidateXmlAsync(string xml)
    {
        try
        {
            var result = await _xmlValidator.ValidateAsync(xml);

            if (result.IsValid)
            {
                _snackbar.Add("XML válido!", Severity.Success);
                return true;
            }
            else
            {
                // Mostrar apenas os primeiros 3 erros para não poluir a UI
                foreach (var error in result.Errors.Take(3))
                {
                    _snackbar.Add($"Erro de validação: {error}", Severity.Warning);
                }

                if (result.Errors.Count > 3)
                {
                    _snackbar.Add($"+ {result.Errors.Count - 3} erro(s) adicional(is)", Severity.Info);
                }

                return false;
            }
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao validar XML: {ex.Message}", Severity.Error);
            return false;
        }
    }

    /// <summary>
    /// Valida XML e retorna resultado detalhado
    /// </summary>
    public async Task<Core.Models.Validation.ValidationResult> ValidateXmlDetailedAsync(string xml)
    {
        try
        {
            var result = await _xmlValidator.ValidateAsync(xml);

            var message = result.IsValid
                ? "Validação concluída com sucesso!"
                : $"Validação encontrou {result.Errors.Count} erro(s)";

            var severity = result.IsValid ? Severity.Success : Severity.Warning;
            _snackbar.Add(message, severity);

            return result;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro na validação: {ex.Message}", Severity.Error);
            return new Core.Models.Validation.ValidationResult
            {
                IsValid = false,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Verifica se o modelo pode ser convertido em XML
    /// </summary>
    public bool CanGenerateXml<T>(T model) where T : class
    {
        return _xmlGenerator.CanGenerateXml(model);
    }

    /// <summary>
    /// Identifica o tipo de documento
    /// </summary>
    public string GetDocumentType<T>(T model) where T : class
    {
        return _xmlGenerator.GetDocumentType(model);
    }

    /// <summary>
    /// Detecta o tipo de documento baseado no XML
    /// </summary>
    public string DetectDocumentTypeFromXml(string xml)
    {
        try
        {
            if (xml.Contains("<DPS") || xml.Contains("DPS"))
                return "DPS";
            if (xml.Contains("<NFSe") || xml.Contains("NFSe"))
                return "NFSe";
            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    /// <summary>
    /// Formata XML para exibição
    /// </summary>
    public string FormatXml(string xml)
    {
        try
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            return doc.ToString();
        }
        catch
        {
            return xml; // Retorna original se não conseguir formatar
        }
    }

    /// <summary>
    /// Extrai informações básicas do XML
    /// </summary>
    public XmlInfo ExtractXmlInfo(string xml)
    {
        try
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            var root = doc.Root;

            return new XmlInfo
            {
                DocumentType = DetectDocumentTypeFromXml(xml),
                RootElement = root?.Name.LocalName ?? "Unknown",
                Namespace = root?.Name.NamespaceName ?? string.Empty,
                Version = root?.Attribute("versao")?.Value ?? "Unknown",
                IsValid = !string.IsNullOrEmpty(root?.Name.LocalName)
            };
        }
        catch (Exception ex)
        {
            return new XmlInfo
            {
                DocumentType = "Invalid",
                RootElement = "Error",
                Namespace = string.Empty,
                Version = "Unknown",
                IsValid = false,
                ErrorMessage = ex.Message
            };
        }
    }

    #endregion
}

/// <summary>
/// Informações extraídas do XML
/// </summary>
public class XmlInfo
{
    public string DocumentType { get; set; } = string.Empty;
    public string RootElement { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}