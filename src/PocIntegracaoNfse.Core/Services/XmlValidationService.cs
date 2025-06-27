using System.Xml;
using PocIntegracaoNfse.Core.Models.Validation;

namespace PocIntegracaoNfse.Core.Services;

/// <summary>
/// Service para validação de XML contra schemas XSD
/// </summary>
public class XmlValidationService
{
    private readonly List<string> _validationErrors = new();
    private readonly List<string> _validationWarnings = new();

    /// <summary>
    /// Valida XML contra os schemas XSD
    /// </summary>
    public async Task<ValidationResult> ValidateAsync(string xmlContent,
        ValidationLevel level = ValidationLevel.Flexible)
    {
        await Task.Delay(100); // Simular processamento assíncrono

        _validationErrors.Clear();
        _validationWarnings.Clear();

        try
        {
            var settings = new XmlReaderSettings();

            // Por enquanto, simular validação
            // TODO: Carregar schemas XSD como recursos embarcados

            using var stringReader = new StringReader(xmlContent);
            using var xmlReader = XmlReader.Create(stringReader, settings);

            var doc = new XmlDocument();
            doc.Load(xmlReader);

            // Validações básicas
            ValidateBasicStructure(doc, level);
            ValidateBusinessRules(doc, level);
            ValidateCompatibilityIssues(doc, level);

            return new ValidationResult
            {
                IsValid = _validationErrors.Count == 0,
                Errors = _validationErrors.ToList(),
                Warnings = _validationWarnings.ToList(),
                ValidationLevel = level
            };
        }
        catch (Exception ex)
        {
            _validationErrors.Add($"Erro ao validar XML: {ex.Message}");
            return new ValidationResult
            {
                IsValid = false,
                Errors = _validationErrors.ToList(),
                Warnings = _validationWarnings.ToList(),
                ValidationLevel = level
            };
        }
    }

    private void ValidateBasicStructure(XmlDocument doc, ValidationLevel level)
    {
        // Validar elemento raiz
        if (doc.DocumentElement?.LocalName != "DPS" && doc.DocumentElement?.LocalName != "NFSe")
        {
            _validationErrors.Add("Elemento raiz deve ser DPS ou NFSe");
        }

        // Validar namespace
        if (doc.DocumentElement?.NamespaceURI != "http://www.sped.fazenda.gov.br/nfse")
        {
            if (level == ValidationLevel.Strict)
                _validationErrors.Add("Namespace incorreto");
            else
                _validationWarnings.Add("Namespace não padrão detectado");
        }

        // Validar versão
        var versao = doc.DocumentElement?.GetAttribute("versao");
        if (versao != "1.00")
        {
            if (level == ValidationLevel.Strict)
                _validationErrors.Add("Versão deve ser 1.00");
            else
                _validationWarnings.Add($"Versão {versao} pode não ser suportada");
        }
    }

    private void ValidateBusinessRules(XmlDocument doc, ValidationLevel level)
    {
        // Validar campos obrigatórios
        var infDps = doc.SelectSingleNode("//*[local-name()='infDPS']");
        if (infDps != null)
        {
            ValidateRequiredField(infDps, "tpAmb", "Tipo de ambiente é obrigatório");
            ValidateRequiredField(infDps, "dhEmi", "Data/hora de emissão é obrigatória");
            ValidateRequiredField(infDps, "serie", "Série é obrigatória");
            ValidateRequiredField(infDps, "nDPS", "Número DPS é obrigatório");
            ValidateRequiredField(infDps, "cLocEmi", "Código local emissor é obrigatório");
        }

        // Validar prestador
        var prestador = doc.SelectSingleNode("//*[local-name()='prest']");
        if (prestador == null)
        {
            _validationErrors.Add("Dados do prestador são obrigatórios");
        }
        else
        {
            ValidatePessoa(prestador, "prestador");
        }

        // Validar serviço
        var servico = doc.SelectSingleNode("//*[local-name()='serv']");
        if (servico == null)
        {
            _validationErrors.Add("Dados do serviço são obrigatórios");
        }

        // Validar valores
        var valores = doc.SelectSingleNode("//*[local-name()='valores']");
        if (valores == null)
        {
            _validationErrors.Add("Valores do serviço são obrigatórios");
        }
    }

    private void ValidateCompatibilityIssues(XmlDocument doc, ValidationLevel level)
    {
        // Verificar se usa tribFed em vez de tribNac
        var tribFed = doc.SelectSingleNode("//*[local-name()='tribFed']");
        if (tribFed != null)
        {
            if (level == ValidationLevel.Strict)
                _validationErrors.Add("Use 'tribNac' em vez de 'tribFed'");
            else
                _validationWarnings.Add("Campo 'tribFed' será mapeado para 'tribNac' automaticamente");
        }

        // Verificar tpEmis vs procEmi
        var tpEmis = doc.SelectSingleNode("//*[local-name()='tpEmis']");
        if (tpEmis != null)
        {
            if (level == ValidationLevel.Strict)
                _validationErrors.Add("Use 'procEmi' em vez de 'tpEmis'");
            else
                _validationWarnings.Add("Campo 'tpEmis' será mapeado para 'procEmi' automaticamente");
        }
    }

    private void ValidateRequiredField(XmlNode parent, string fieldName, string errorMessage)
    {
        var field = parent.SelectSingleNode($"*[local-name()='{fieldName}']");
        if (field == null || string.IsNullOrWhiteSpace(field.InnerText))
        {
            _validationErrors.Add(errorMessage);
        }
    }

    private void ValidatePessoa(XmlNode pessoa, string tipo)
    {
        var cnpj = pessoa.SelectSingleNode("*[local-name()='CNPJ']");
        var cpf = pessoa.SelectSingleNode("*[local-name()='CPF']");
        var nif = pessoa.SelectSingleNode("*[local-name()='NIF']");

        if (cnpj == null && cpf == null && nif == null)
        {
            _validationErrors.Add($"Documento de identificação é obrigatório para {tipo}");
        }

        // Validar CNPJ
        if (cnpj != null && !string.IsNullOrWhiteSpace(cnpj.InnerText))
        {
            if (cnpj.InnerText.Length != 14 || !cnpj.InnerText.All(char.IsDigit))
            {
                _validationErrors.Add($"CNPJ inválido para {tipo}");
            }
        }

        // Validar CPF
        if (cpf != null && !string.IsNullOrWhiteSpace(cpf.InnerText))
        {
            if (cpf.InnerText.Length != 11 || !cpf.InnerText.All(char.IsDigit))
            {
                _validationErrors.Add($"CPF inválido para {tipo}");
            }
        }
    }
}