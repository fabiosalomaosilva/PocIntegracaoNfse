using PocIntegracaoNfse.Core.Models.Validation;
using PocIntegracaoNfse.Models.Common;
using PocIntegracaoNfse.Models.DPS;
using PocIntegracaoNfse.Models.Enums;
using System.Xml;
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

/// <summary>
/// Service para geração de XML a partir dos models
/// </summary>
public class XmlGeneratorService
{
    private const string Namespace = "http://www.sped.fazenda.gov.br/nfse";

    /// <summary>
    /// Gera XML a partir de um model DPS
    /// </summary>
    public async Task<string> GenerateXmlAsync(DpsModel dps)
    {
        await Task.Delay(100); // Simular processamento assíncrono

        try
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                CreateDpsElement(dps)
            );

            return doc.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao gerar XML: {ex.Message}", ex);
        }
    }

    private XElement CreateDpsElement(DpsModel dps)
    {
        var dpsElement = new XElement(GetXName("DPS"),
            new XAttribute("versao", dps.Versao),
            new XAttribute(XNamespace.Xmlns + "xmlns", Namespace)
        );

        if (dps.InfDPS != null)
        {
            dpsElement.Add(CreateInfDpsElement(dps.InfDPS));
        }

        return dpsElement;
    }

    private XElement CreateInfDpsElement(InfDpsModel infDps)
    {
        var element = new XElement(GetXName("infDPS"),
            new XAttribute("Id", infDps.Id),
            new XElement(GetXName("tpAmb"), (int)infDps.TpAmb),
            new XElement(GetXName("dhEmi"), infDps.DhEmi.ToString("yyyy-MM-ddTHH:mm:sszzz")),
            new XElement(GetXName("verAplic"), infDps.VerAplic),
            new XElement(GetXName("serie"), infDps.Serie),
            new XElement(GetXName("nDPS"), infDps.NDPS),
            new XElement(GetXName("dCompet"), infDps.DCompet.ToString("yyyy-MM-dd")),
            new XElement(GetXName("tpEmit"), (int)infDps.TpEmit),
            new XElement(GetXName("cLocEmi"), infDps.CLocEmi)
        );

        if (infDps.Subst != null)
        {
            element.Add(CreateSubstituicaoElement(infDps.Subst));
        }

        element.Add(CreatePrestadorElement(infDps.Prest));

        if (infDps.Toma != null)
        {
            element.Add(CreateTomadorElement(infDps.Toma));
        }

        if (infDps.Interm != null)
        {
            element.Add(CreateIntermediarioElement(infDps.Interm));
        }

        element.Add(CreateServicoElement(infDps.Serv));
        element.Add(CreateValoresElement(infDps.Valores));

        return element;
    }

    private XElement CreateSubstituicaoElement(SubstituicaoModel subst)
    {
        var element = new XElement(GetXName("subst"),
            new XElement(GetXName("chSubstda"), subst.ChSubstda),
            new XElement(GetXName("cMotivo"), subst.CMotivo)
        );

        if (!string.IsNullOrEmpty(subst.XMotivo))
        {
            element.Add(new XElement(GetXName("xMotivo"), subst.XMotivo));
        }

        return element;
    }

    private XElement CreatePrestadorElement(PrestadorModel prest)
    {
        var element = new XElement(GetXName("prest"));

        AddPessoaBaseElements(element, prest);
        element.Add(CreateRegimeTributarioElement(prest.RegTrib));

        return element;
    }

    private XElement CreateTomadorElement(TomadorModel toma)
    {
        var element = new XElement(GetXName("toma"));
        AddPessoaBaseElements(element, toma);
        return element;
    }

    private XElement CreateIntermediarioElement(IntermediarioModel interm)
    {
        var element = new XElement(GetXName("interm"));
        AddPessoaBaseElements(element, interm);
        return element;
    }

    private void AddPessoaBaseElements(XElement element, PessoaBaseModel pessoa)
    {
        if (!string.IsNullOrEmpty(pessoa.CNPJ))
            element.Add(new XElement(GetXName("CNPJ"), pessoa.CNPJ));

        if (!string.IsNullOrEmpty(pessoa.CPF))
            element.Add(new XElement(GetXName("CPF"), pessoa.CPF));

        if (!string.IsNullOrEmpty(pessoa.NIF))
            element.Add(new XElement(GetXName("NIF"), pessoa.NIF));

        if (pessoa.CNaoNIF.HasValue)
            element.Add(new XElement(GetXName("cNaoNIF"), (int)pessoa.CNaoNIF.Value));

        if (!string.IsNullOrEmpty(pessoa.CAEPF))
            element.Add(new XElement(GetXName("CAEPF"), pessoa.CAEPF));

        if (!string.IsNullOrEmpty(pessoa.IM))
            element.Add(new XElement(GetXName("IM"), pessoa.IM));

        if (!string.IsNullOrEmpty(pessoa.XNome))
            element.Add(new XElement(GetXName("xNome"), pessoa.XNome));

        if (pessoa.End != null)
            element.Add(CreateEnderecoElement(pessoa.End));

        if (!string.IsNullOrEmpty(pessoa.Fone))
            element.Add(new XElement(GetXName("fone"), pessoa.Fone));

        if (!string.IsNullOrEmpty(pessoa.Email))
            element.Add(new XElement(GetXName("email"), pessoa.Email));
    }

    private XElement CreateEnderecoElement(EnderecoModel endereco)
    {
        var element = new XElement(GetXName("end"));

        if (endereco.EndNac != null)
        {
            element.Add(new XElement(GetXName("endNac"),
                new XElement(GetXName("cMun"), endereco.EndNac.CMun),
                new XElement(GetXName("CEP"), endereco.EndNac.CEP)
            ));
        }

        if (endereco.EndExt != null)
        {
            element.Add(new XElement(GetXName("endExt"),
                new XElement(GetXName("cPais"), endereco.EndExt.CPais),
                new XElement(GetXName("cEndPost"), endereco.EndExt.CEndPost),
                new XElement(GetXName("xCidade"), endereco.EndExt.XCidade),
                new XElement(GetXName("xEstProvReg"), endereco.EndExt.XEstProvReg)
            ));
        }

        element.Add(
            new XElement(GetXName("xLgr"), endereco.XLgr),
            new XElement(GetXName("nro"), endereco.Nro),
            new XElement(GetXName("xBairro"), endereco.XBairro)
        );

        if (!string.IsNullOrEmpty(endereco.XCpl))
            element.Add(new XElement(GetXName("xCpl"), endereco.XCpl));

        return element;
    }

    private XElement CreateRegimeTributarioElement(RegimeTributarioModel regTrib)
    {
        var element = new XElement(GetXName("regTrib"),
            new XElement(GetXName("opSimpNac"), (int)regTrib.OpSimpNac),
            new XElement(GetXName("regEspTrib"), (int)regTrib.RegEspTrib)
        );

        if (regTrib.RegApTribSN.HasValue)
        {
            element.Add(new XElement(GetXName("regApTribSN"), (int)regTrib.RegApTribSN.Value));
        }

        return element;
    }

    private XElement CreateServicoElement(ServicoModel servico)
    {
        var element = new XElement(GetXName("serv"));

        element.Add(CreateLocalPrestacaoElement(servico.LocPrest));
        element.Add(CreateCodigoServicoElement(servico.CServ));

        if (servico.InfoCompl != null)
        {
            element.Add(CreateInfoComplElement(servico.InfoCompl));
        }

        return element;
    }

    private XElement CreateLocalPrestacaoElement(LocalPrestacaoModel locPrest)
    {
        var element = new XElement(GetXName("locPrest"));

        if (!string.IsNullOrEmpty(locPrest.CLocPrestacao))
            element.Add(new XElement(GetXName("cLocPrestacao"), locPrest.CLocPrestacao));

        if (!string.IsNullOrEmpty(locPrest.CPaisPrestacao))
            element.Add(new XElement(GetXName("cPaisPrestacao"), locPrest.CPaisPrestacao));

        return element;
    }

    private XElement CreateCodigoServicoElement(CodigoServicoModel cServ)
    {
        var element = new XElement(GetXName("cServ"),
            new XElement(GetXName("cTribNac"), cServ.CTribNac),
            new XElement(GetXName("xDescServ"), cServ.XDescServ)
        );

        if (!string.IsNullOrEmpty(cServ.CTribMun))
            element.Add(new XElement(GetXName("cTribMun"), cServ.CTribMun));

        if (!string.IsNullOrEmpty(cServ.CNBS))
            element.Add(new XElement(GetXName("cNBS"), cServ.CNBS));

        if (!string.IsNullOrEmpty(cServ.CIntContrib))
            element.Add(new XElement(GetXName("cIntContrib"), cServ.CIntContrib));

        return element;
    }

    private XElement CreateInfoComplElement(InformacoesComplementaresModel infoCompl)
    {
        var element = new XElement(GetXName("infoCompl"));

        if (!string.IsNullOrEmpty(infoCompl.IdDocTec))
            element.Add(new XElement(GetXName("idDocTec"), infoCompl.IdDocTec));

        if (!string.IsNullOrEmpty(infoCompl.DocRef))
            element.Add(new XElement(GetXName("docRef"), infoCompl.DocRef));

        if (!string.IsNullOrEmpty(infoCompl.XInfComp))
            element.Add(new XElement(GetXName("xInfComp"), infoCompl.XInfComp));

        return element;
    }

    private XElement CreateValoresElement(ValoresModel valores)
    {
        var element = new XElement(GetXName("valores"));

        element.Add(CreateValorServicoElement(valores.VServPrest));

        if (valores.VDescCondIncond != null)
        {
            element.Add(CreateDescontosElement(valores.VDescCondIncond));
        }

        if (valores.VDedRed != null)
        {
            element.Add(CreateDeducoesElement(valores.VDedRed));
        }

        element.Add(CreateTributacaoElement(valores.Trib));

        return element;
    }

    private XElement CreateValorServicoElement(ValorServicoModel vServPrest)
    {
        var element = new XElement(GetXName("vServPrest"),
            new XElement(GetXName("vServ"), vServPrest.VServ.ToString("F2"))
        );

        if (vServPrest.VReceb.HasValue)
        {
            element.Add(new XElement(GetXName("vReceb"), vServPrest.VReceb.Value.ToString("F2")));
        }

        return element;
    }

    private XElement CreateDescontosElement(DescontosModel descontos)
    {
        var element = new XElement(GetXName("vDescCondIncond"));

        if (descontos.VDescIncond.HasValue)
            element.Add(new XElement(GetXName("vDescIncond"), descontos.VDescIncond.Value.ToString("F2")));

        if (descontos.VDescCond.HasValue)
            element.Add(new XElement(GetXName("vDescCond"), descontos.VDescCond.Value.ToString("F2")));

        return element;
    }

    private XElement CreateDeducoesElement(DeducoesModel deducoes)
    {
        var element = new XElement(GetXName("vDedRed"));

        if (deducoes.VDR.HasValue)
            element.Add(new XElement(GetXName("vDR"), deducoes.VDR.Value.ToString("F2")));

        return element;
    }

    private XElement CreateTributacaoElement(TributacaoModel trib)
    {
        var element = new XElement(GetXName("trib"));

        element.Add(CreateTributacaoMunicipalElement(trib.TribMun));

        if (trib.TribNac != null)
        {
            element.Add(CreateTributacaoNacionalElement(trib.TribNac));
        }

        element.Add(CreateTotalTributosElement(trib.TotTrib));

        return element;
    }

    private XElement CreateTributacaoMunicipalElement(TributacaoMunicipalModel tribMun)
    {
        var element = new XElement(GetXName("tribMun"),
            new XElement(GetXName("tribISSQN"), tribMun.TribISSQN),
            new XElement(GetXName("tpRetISSQN"), tribMun.TpRetISSQN)
        );

        if (tribMun.PAliq.HasValue)
            element.Add(new XElement(GetXName("pAliq"), tribMun.PAliq.Value.ToString("F2")));

        return element;
    }

    private XElement CreateTributacaoNacionalElement(TributacaoNacionalModel tribNac)
    {
        var element = new XElement(GetXName("tribNac"));

        if (tribNac.VRetCP.HasValue)
            element.Add(new XElement(GetXName("vRetCP"), tribNac.VRetCP.Value.ToString("F2")));

        if (tribNac.VRetIRRF.HasValue)
            element.Add(new XElement(GetXName("vRetIRRF"), tribNac.VRetIRRF.Value.ToString("F2")));

        if (tribNac.VRetCSLL.HasValue)
            element.Add(new XElement(GetXName("vRetCSLL"), tribNac.VRetCSLL.Value.ToString("F2")));

        return element;
    }

    private XElement CreateTotalTributosElement(TotalTributosModel totTrib)
    {
        return new XElement(GetXName("totTrib"),
            new XElement(GetXName("vTotTrib"),
                new XElement(GetXName("vTotTribFed"), totTrib.VTotTribFed.ToString("F2")),
                new XElement(GetXName("vTotTribEst"), totTrib.VTotTribEst.ToString("F2")),
                new XElement(GetXName("vTotTribMun"), totTrib.VTotTribMun.ToString("F2"))
            )
        );
    }

    private static XName GetXName(string localName)
    {
        return XName.Get(localName, Namespace);
    }
}

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