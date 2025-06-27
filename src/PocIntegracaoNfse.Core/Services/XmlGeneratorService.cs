using PocIntegracaoNfse.Models.DPS;
using PocIntegracaoNfse.Models.NFSe;
using System.Xml.Linq;

namespace PocIntegracaoNfse.Core.Services;

/// <summary>
/// Extensão do XmlGeneratorService para suporte a NFSe
/// </summary>
public partial class XmlGeneratorService
{
    private const string Namespace = "http://www.sped.fazenda.gov.br/nfse";

    /// <summary>
    /// Gera XML a partir de um model NFSe
    /// </summary>
    public async Task<string> GenerateNfseXmlAsync(NfseModel nfse)
    {
        await Task.Delay(100); // Simular processamento assíncrono

        try
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                CreateNfseElement(nfse)
            );

            return doc.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao gerar XML NFSe: {ex.Message}", ex);
        }
    }

    #region NFSe XML Generation

    private async Task<XElement> CreateNfseElement(NfseModel nfse)
    {
        var nfseElement = new XElement(GetXName("NFSe"),
            new XAttribute("versao", nfse.Versao),
            new XAttribute(XNamespace.Xmlns + "xmlns", Namespace)
        );

        if (nfse.InfNFSe != null)
        {
            nfseElement.Add(await CreateInfNfseElement(nfse.InfNFSe));
        }

        // TODO: Adicionar assinatura digital quando implementada
        // if (nfse.Signature != null)
        // {
        //     nfseElement.Add(CreateSignatureElement(nfse.Signature));
        // }

        return nfseElement;
    }

    private async Task<XElement> CreateInfNfseElement(InfNfseModel infNfse)
    {
        var element = new XElement(GetXName("infNFSe"),
            new XAttribute("Id", infNfse.Id),
            new XElement(GetXName("xLocEmi"), infNfse.XLocEmi),
            new XElement(GetXName("xLocPrestacao"), infNfse.XLocPrestacao),
            new XElement(GetXName("nNFSe"), infNfse.NNFSe),
            new XElement(GetXName("verAplic"), infNfse.VerAplic),
            new XElement(GetXName("ambGer"), (int)infNfse.AmbGer),
            new XElement(GetXName("tpEmis"), (int)infNfse.TpEmis),
            new XElement(GetXName("procEmi"), (int)infNfse.ProcEmi),
            new XElement(GetXName("cStat"), infNfse.CStat),
            new XElement(GetXName("dhProc"), infNfse.DhProc.ToString("yyyy-MM-ddTHH:mm:sszzz")),
            new XElement(GetXName("nDFSe"), infNfse.NDFSe)
        );

        // Campos opcionais
        if (!string.IsNullOrEmpty(infNfse.CLocIncid))
        {
            element.Add(new XElement(GetXName("cLocIncid"), infNfse.CLocIncid));
        }

        if (!string.IsNullOrEmpty(infNfse.XLocIncid))
        {
            element.Add(new XElement(GetXName("xLocIncid"), infNfse.XLocIncid));
        }

        if (!string.IsNullOrEmpty(infNfse.XTribNac))
        {
            element.Add(new XElement(GetXName("xTribNac"), infNfse.XTribNac));
        }

        if (!string.IsNullOrEmpty(infNfse.XTribMun))
        {
            element.Add(new XElement(GetXName("xTribMun"), infNfse.XTribMun));
        }

        if (!string.IsNullOrEmpty(infNfse.XNBS))
        {
            element.Add(new XElement(GetXName("xNBS"), infNfse.XNBS));
        }

        // Elementos obrigatórios
        element.Add(CreateEmitenteNfseElement(infNfse.Emit));
        element.Add(CreateValoresNfseElement(infNfse.Valores));

        // DPS que originou a NFSe - usar o método original da classe
        if (infNfse.DPS != null)
        {
            element.Add(await CreateDpsElementAsync(infNfse.DPS));
        }

        return element;
    }

    /// <summary>
    /// Método auxiliar para criar elemento DPS dentro da NFSe
    /// </summary>
    private async Task<XElement> CreateDpsElementAsync(DpsModel dps)
    {
        // Reutilizar o método existente da classe para gerar DPS
        var dpsXml = await GenerateXmlAsync(dps);
        var dpsDoc = XDocument.Parse(dpsXml);
        return dpsDoc.Root ?? new XElement(GetXName("DPS"));
    }

    private XElement CreateEmitenteNfseElement(EmitenteNfseModel emit)
    {
        var element = new XElement(GetXName("emit"));

        // Documento de identificação (CNPJ ou CPF)
        if (!string.IsNullOrEmpty(emit.CNPJ))
        {
            element.Add(new XElement(GetXName("CNPJ"), emit.CNPJ));
        }
        else if (!string.IsNullOrEmpty(emit.CPF))
        {
            element.Add(new XElement(GetXName("CPF"), emit.CPF));
        }

        // Inscrição Municipal
        if (!string.IsNullOrEmpty(emit.IM))
        {
            element.Add(new XElement(GetXName("IM"), emit.IM));
        }

        // Nome/Razão Social
        element.Add(new XElement(GetXName("xNome"), emit.XNome));

        // Nome Fantasia
        if (!string.IsNullOrEmpty(emit.XFant))
        {
            element.Add(new XElement(GetXName("xFant"), emit.XFant));
        }

        // Endereço Nacional
        element.Add(CreateEnderecoEmitenteElement(emit.EnderNac));

        // Telefone
        if (!string.IsNullOrEmpty(emit.Fone))
        {
            element.Add(new XElement(GetXName("fone"), emit.Fone));
        }

        // E-mail
        if (!string.IsNullOrEmpty(emit.Email))
        {
            element.Add(new XElement(GetXName("email"), emit.Email));
        }

        return element;
    }

    private XElement CreateEnderecoEmitenteElement(EnderecoEmitenteModel enderNac)
    {
        var element = new XElement(GetXName("enderNac"),
            new XElement(GetXName("xLgr"), enderNac.XLgr),
            new XElement(GetXName("nro"), enderNac.Nro),
            new XElement(GetXName("xBairro"), enderNac.XBairro),
            new XElement(GetXName("cMun"), enderNac.CMun),
            new XElement(GetXName("UF"), enderNac.UF.ToString()),
            new XElement(GetXName("CEP"), enderNac.CEP)
        );

        // Complemento (opcional)
        if (!string.IsNullOrEmpty(enderNac.XCpl))
        {
            element.Add(new XElement(GetXName("xCpl"), enderNac.XCpl));
        }

        return element;
    }

    private XElement CreateValoresNfseElement(ValoresNfseModel valores)
    {
        var element = new XElement(GetXName("valores"),
            new XElement(GetXName("vLiq"), valores.VLiq.ToString("F2"))
        );

        // Campos opcionais
        if (valores.VCalcDR.HasValue)
        {
            element.Add(new XElement(GetXName("vCalcDR"), valores.VCalcDR.Value.ToString("F2")));
        }

        if (!string.IsNullOrEmpty(valores.TpBM))
        {
            element.Add(new XElement(GetXName("tpBM"), valores.TpBM));
        }

        if (valores.VCalcBM.HasValue)
        {
            element.Add(new XElement(GetXName("vCalcBM"), valores.VCalcBM.Value.ToString("F2")));
        }

        if (valores.VBC.HasValue)
        {
            element.Add(new XElement(GetXName("vBC"), valores.VBC.Value.ToString("F2")));
        }

        if (valores.PAliqAplic.HasValue)
        {
            element.Add(new XElement(GetXName("pAliqAplic"), valores.PAliqAplic.Value.ToString("F2")));
        }

        if (valores.VISSQN.HasValue)
        {
            element.Add(new XElement(GetXName("vISSQN"), valores.VISSQN.Value.ToString("F2")));
        }

        if (valores.VTotalRet.HasValue)
        {
            element.Add(new XElement(GetXName("vTotalRet"), valores.VTotalRet.Value.ToString("F2")));
        }

        if (!string.IsNullOrEmpty(valores.XOutInf))
        {
            element.Add(new XElement(GetXName("xOutInf"), valores.XOutInf));
        }

        return element;
    }

    #endregion

    #region Overload Methods for Service Integration

    /// <summary>
    /// Gera XML com detecção automática do tipo (DPS ou NFSe)
    /// </summary>
    public async Task<string> GenerateXmlAsync(object model)
    {
        return model switch
        {
            DpsModel dps => await GenerateXmlAsync(dps),
            NfseModel nfse => await GenerateNfseXmlAsync(nfse),
            _ => throw new ArgumentException($"Tipo de modelo não suportado: {model.GetType().Name}")
        };
    }

    /// <summary>
    /// Gera XML com base no tipo especificado
    /// </summary>
    public async Task<string> GenerateXmlAsync<T>(T model) where T : class
    {
        return model switch
        {
            DpsModel dps => await GenerateXmlAsync(dps),
            NfseModel nfse => await GenerateNfseXmlAsync(nfse),
            _ => throw new ArgumentException($"Tipo de modelo não suportado: {typeof(T).Name}")
        };
    }

    /// <summary>
    /// Valida se o modelo pode ser convertido em XML
    /// </summary>
    public bool CanGenerateXml<T>(T model) where T : class
    {
        return model is DpsModel or NfseModel;
    }

    /// <summary>
    /// Obtém informações sobre o tipo de documento
    /// </summary>
    public string GetDocumentType<T>(T model) where T : class
    {
        return model switch
        {
            DpsModel => "DPS",
            NfseModel => "NFSe",
            _ => "Unknown"
        };
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Cria XName com namespace NFSe
    /// </summary>
    private static XName GetXName(string localName)
    {
        return XName.Get(localName, Namespace);
    }

    /// <summary>
    /// Formata valores monetários para XML
    /// </summary>
    private static string FormatCurrency(decimal value)
    {
        return value.ToString("F2");
    }

    /// <summary>
    /// Formata percentuais para XML
    /// </summary>
    private static string FormatPercentage(decimal value)
    {
        return value.ToString("F2");
    }

    /// <summary>
    /// Formata datas para XML (UTC)
    /// </summary>
    private static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }

    /// <summary>
    /// Formata datas simples para XML
    /// </summary>
    private static string FormatDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Cria elemento apenas se o valor não for nulo/vazio
    /// </summary>
    private static XElement? CreateOptionalElement(string elementName, string? value)
    {
        return string.IsNullOrEmpty(value) ? null : new XElement(GetXName(elementName), value);
    }

    /// <summary>
    /// Cria elemento apenas se o valor for maior que zero
    /// </summary>
    private static XElement? CreateOptionalElement(string elementName, decimal? value)
    {
        return value.HasValue && value.Value > 0 ? new XElement(GetXName(elementName), FormatCurrency(value.Value)) : null;
    }

    #endregion
}