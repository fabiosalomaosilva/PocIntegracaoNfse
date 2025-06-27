using MudBlazor;
using PocIntegracaoNfse.Core.Services;
using PocIntegracaoNfse.Models.DPS;

namespace PocIntegracaoNfse.Services;
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

    public async Task<string> GenerateXmlAsync(DpsModel dps)
    {
        try
        {
            var xml = await _xmlGenerator.GenerateXmlAsync(dps);
            _snackbar.Add("XML gerado com sucesso!", Severity.Success);
            return xml;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao gerar XML: {ex.Message}", Severity.Error);
            throw;
        }
    }

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
                foreach (var error in result.Errors.Take(3)) // Mostrar apenas os primeiros 3 erros
                {
                    _snackbar.Add($"Erro de validação: {error}", Severity.Warning);
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

    public async Task<DpsModel?> ParseXmlAsync(string xml)
    {
        try
        {
            var dps = await _xmlParser.ParseDpsAsync(xml);
            _snackbar.Add("XML importado com sucesso!", Severity.Success);
            return dps;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Erro ao importar XML: {ex.Message}", Severity.Error);
            return null;
        }
    }

}

