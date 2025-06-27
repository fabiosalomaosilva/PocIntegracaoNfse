using System.ComponentModel.DataAnnotations;

namespace PocIntegracaoNfse.Models.DPS;

/// <summary>
/// Dados do serviço prestado
/// </summary>
public class ServicoModel
{
    /// <summary>Local da prestação</summary>
    [Required]
    public LocalPrestacaoModel LocPrest { get; set; } = new();

    /// <summary>Código do serviço</summary>
    [Required]
    public CodigoServicoModel CServ { get; set; } = new();

    /// <summary>Informações complementares</summary>
    public InformacoesComplementaresModel? InfoCompl { get; set; }
}