namespace PocIntegracaoNfse.Models.Constants;

/// <summary>
/// Mensagens de validação padrão
/// </summary>
public static class ValidationMessages
{
    // Mensagens gerais
    public const string CampoObrigatorio = "O campo {0} é obrigatório.";
    public const string TamanhoMaximo = "O campo {0} deve ter no máximo {1} caracteres.";
    public const string TamanhoMinimo = "O campo {0} deve ter no mínimo {1} caracteres.";
    public const string FormatoInvalido = "O campo {0} possui formato inválido.";

    // Validações específicas
    public const string CNPJInvalido = "CNPJ inválido.";
    public const string CPFInvalido = "CPF inválido.";
    public const string CEPInvalido = "CEP deve conter 8 dígitos.";
    public const string EmailInvalido = "E-mail possui formato inválido.";
    public const string TelefoneInvalido = "Telefone deve conter entre 6 e 20 dígitos.";
    public const string DataInvalida = "Data inválida.";
    public const string ValorInvalido = "Valor deve ser maior que zero.";
    public const string CodigoMunicipalInvalido = "Código municipal deve conter 7 dígitos.";

    // Validações de negócio
    public const string DataEmissaoFutura = "Data de emissão não pode ser futura.";
    public const string DataCompetenciaInvalida = "Data de competência inválida.";
    public const string ValorServicoObrigatorio = "Valor do serviço é obrigatório quando há tributação.";
    public const string AliquotaObrigatoria = "Alíquota é obrigatória para operações tributáveis.";
    public const string ProcessoExigibilidadeObrigatorio = "Número do processo é obrigatório para exigibilidade suspensa.";

    // Compatibilidade com XML base
    public const string CampoNaoConforme = "Campo {0} não está em conformidade com o schema XSD, mas será aceito por compatibilidade.";
    public const string EstruturaNaoConforme = "Estrutura {0} difere do schema XSD, mas será aceita por compatibilidade.";
}