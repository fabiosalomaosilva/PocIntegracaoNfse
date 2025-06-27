using PocIntegracaoNfse.Models.Common;
using PocIntegracaoNfse.Models.DPS;
using PocIntegracaoNfse.Models.Enums;
using System.Xml.Linq;

namespace PocIntegracaoNfse.Core.Services;

/// <summary>
/// Service para parsing de XML para models
/// </summary>
public class XmlParserService
{
    /// <summary>
    /// Faz o parse de um XML DPS para o model
    /// </summary>
    public async Task<DpsModel> ParseDpsAsync(string xmlContent)
    {
        await Task.Delay(100); // Simular processamento assíncrono

        try
        {
            var doc = XDocument.Parse(xmlContent);
            var dpsElement = doc.Root;

            if (dpsElement?.Name.LocalName != "DPS")
                throw new InvalidOperationException("XML não é uma DPS válida");

            var dps = new DpsModel
            {
                Versao = dpsElement.Attribute("versao")?.Value ?? "1.00"
            };

            var infDpsElement = dpsElement.Element(GetXName("infDPS"));
            if (infDpsElement != null)
            {
                dps.InfDPS = ParseInfDps(infDpsElement);
            }

            return dps;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao fazer parse do XML: {ex.Message}", ex);
        }
    }

    private InfDpsModel ParseInfDps(XElement infDpsElement)
    {
        var infDps = new InfDpsModel
        {
            Id = infDpsElement.Attribute("Id")?.Value ?? string.Empty,
            TpAmb = GetEnumValue<TipoAmbiente>(infDpsElement, "tpAmb"),
            VerAplic = GetElementValue(infDpsElement, "verAplic") ?? "1.00",
            Serie = GetElementValue(infDpsElement, "serie") ?? "00001",
            NDPS = GetElementValue(infDpsElement, "nDPS") ?? "1",
            CLocEmi = GetElementValue(infDpsElement, "cLocEmi") ?? string.Empty
        };

        // Parse data/hora emissão
        if (DateTime.TryParse(GetElementValue(infDpsElement, "dhEmi"), out var dhEmi))
            infDps.DhEmi = dhEmi;

        // Parse data competência
        if (DateOnly.TryParse(GetElementValue(infDpsElement, "dCompet"), out var dCompet))
            infDps.DCompet = dCompet;

        // Parse prestador
        var prestElement = infDpsElement.Element(GetXName("prest"));
        if (prestElement != null)
        {
            infDps.Prest = ParsePrestador(prestElement);
        }

        // Parse tomador
        var tomaElement = infDpsElement.Element(GetXName("toma"));
        if (tomaElement != null)
        {
            infDps.Toma = ParseTomador(tomaElement);
        }

        // Parse serviço
        var servElement = infDpsElement.Element(GetXName("serv"));
        if (servElement != null)
        {
            infDps.Serv = ParseServico(servElement);
        }

        // Parse valores
        var valoresElement = infDpsElement.Element(GetXName("valores"));
        if (valoresElement != null)
        {
            infDps.Valores = ParseValores(valoresElement);
        }

        return infDps;
    }

    private PrestadorModel ParsePrestador(XElement prestElement)
    {
        var prestador = new PrestadorModel();
        ParsePessoaBase(prestElement, prestador);

        // Parse regimes tributários
        var regTribElement = prestElement.Element(GetXName("regTrib"));
        if (regTribElement != null)
        {
            prestador.RegTrib = ParseRegimeTributario(regTribElement);
        }

        return prestador;
    }

    private TomadorModel ParseTomador(XElement tomaElement)
    {
        var tomador = new TomadorModel();
        ParsePessoaBase(tomaElement, tomador);
        return tomador;
    }

    private void ParsePessoaBase(XElement element, PessoaBaseModel pessoa)
    {
        pessoa.CNPJ = GetElementValue(element, "CNPJ");
        pessoa.CPF = GetElementValue(element, "CPF");
        pessoa.IM = GetElementValue(element, "IM");
        pessoa.XNome = GetElementValue(element, "xNome");
        pessoa.Fone = GetElementValue(element, "fone");
        pessoa.Email = GetElementValue(element, "email");
    }

    private RegimeTributarioModel ParseRegimeTributario(XElement regTribElement)
    {
        return new RegimeTributarioModel
        {
            OpSimpNac = GetEnumValue<SituacaoSimplesNacional>(regTribElement, "opSimpNac"),
            RegEspTrib = GetEnumValue<RegimeEspecialTributacao>(regTribElement, "regEspTrib")
        };
    }

    private ServicoModel ParseServico(XElement servElement)
    {
        var servico = new ServicoModel();

        var locPrestElement = servElement.Element(GetXName("locPrest"));
        if (locPrestElement != null)
        {
            servico.LocPrest = new LocalPrestacaoModel
            {
                CLocPrestacao = GetElementValue(locPrestElement, "cLocPrestacao")
            };
        }

        var cServElement = servElement.Element(GetXName("cServ"));
        if (cServElement != null)
        {
            servico.CServ = new CodigoServicoModel
            {
                CTribNac = GetElementValue(cServElement, "cTribNac") ?? string.Empty,
                XDescServ = GetElementValue(cServElement, "xDescServ") ?? string.Empty
            };
        }

        return servico;
    }

    private ValoresModel ParseValores(XElement valoresElement)
    {
        var valores = new ValoresModel();

        var vServPrestElement = valoresElement.Element(GetXName("vServPrest"));
        if (vServPrestElement != null)
        {
            valores.VServPrest = new ValorServicoModel
            {
                VServ = GetDecimalValue(vServPrestElement, "vServ") ?? 0
            };
        }

        var tribElement = valoresElement.Element(GetXName("trib"));
        if (tribElement != null)
        {
            valores.Trib = ParseTributacao(tribElement);
        }

        return valores;
    }

    private TributacaoModel ParseTributacao(XElement tribElement)
    {
        var trib = new TributacaoModel();

        var tribMunElement = tribElement.Element(GetXName("tribMun"));
        if (tribMunElement != null)
        {
            trib.TribMun = new TributacaoMunicipalModel
            {
                TribISSQN = GetIntValue(tribMunElement, "tribISSQN") ?? 1,
                PAliq = GetDecimalValue(tribMunElement, "pAliq"),
                TpRetISSQN = GetIntValue(tribMunElement, "tpRetISSQN") ?? 1
            };
        }

        // Parse tributos nacionais/federais (compatibilidade)
        var tribNacElement = tribElement.Element(GetXName("tribNac")) ??
                            tribElement.Element(GetXName("tribFed")); // Compatibilidade
        if (tribNacElement != null)
        {
            trib.TribNac = new TributacaoNacionalModel
            {
                VRetCP = GetDecimalValue(tribNacElement, "vRetCP"),
                VRetIRRF = GetDecimalValue(tribNacElement, "vRetIRRF"),
                VRetCSLL = GetDecimalValue(tribNacElement, "vRetCSLL")
            };
        }

        var totTribElement = tribElement.Element(GetXName("totTrib"));
        if (totTribElement != null)
        {
            var vTotTribElement = totTribElement.Element(GetXName("vTotTrib"));
            if (vTotTribElement != null)
            {
                trib.TotTrib = new TotalTributosModel
                {
                    VTotTribFed = GetDecimalValue(vTotTribElement, "vTotTribFed") ?? 0,
                    VTotTribEst = GetDecimalValue(vTotTribElement, "vTotTribEst") ?? 0,
                    VTotTribMun = GetDecimalValue(vTotTribElement, "vTotTribMun") ?? 0
                };
            }
        }

        return trib;
    }

    // Métodos auxiliares
    private static XName GetXName(string localName)
    {
        return XName.Get(localName, "http://www.sped.fazenda.gov.br/nfse");
    }

    private static string? GetElementValue(XElement parent, string elementName)
    {
        return parent.Element(GetXName(elementName))?.Value;
    }

    private static T GetEnumValue<T>(XElement parent, string elementName) where T : struct, Enum
    {
        var value = GetElementValue(parent, elementName);
        if (value != null && Enum.TryParse<T>(value, out var result))
            return result;
        return default;
    }

    private static decimal? GetDecimalValue(XElement parent, string elementName)
    {
        var value = GetElementValue(parent, elementName);
        if (value != null && decimal.TryParse(value, out var result))
            return result;
        return null;
    }

    private static int? GetIntValue(XElement parent, string elementName)
    {
        var value = GetElementValue(parent, elementName);
        if (value != null && int.TryParse(value, out var result))
            return result;
        return null;
    }
}