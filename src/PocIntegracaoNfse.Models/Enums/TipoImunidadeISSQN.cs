namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Identificação da Imunidade do ISSQN
/// Baseado em TSTipoImunidadeISSQN
/// </summary>
public enum TipoImunidadeISSQN
{
    /// <summary>0 - Imunidade (tipo não informado na nota de origem)</summary>
    TipoNaoInformado = 0,

    /// <summary>1 - Patrimônio, renda ou serviços, uns dos outros (CF88, Art 150, VI, a)</summary>
    PatrimonioRendaServicos = 1,

    /// <summary>2 - Templos de qualquer culto (CF88, Art 150, VI, b)</summary>
    TemplosReligiosos = 2,

    /// <summary>3 - Patrimônio, renda ou serviços dos partidos políticos, inclusive suas fundações, das entidades sindicais dos trabalhadores, das instituições de educação e de assistência social, sem fins lucrativos, atendidos os requisitos da lei (CF88, Art 150, VI, c)</summary>
    PartidosEntidadesSociais = 3,

    /// <summary>4 - Livros, jornais, periódicos e o papel destinado a sua impressão (CF88, Art 150, VI, d)</summary>
    LivrosJornaisPeriodicos = 4,

    /// <summary>5 - Fonogramas e videofonogramas musicais produzidos no Brasil contendo obras musicais ou literomusicais de autores brasileiros e/ou obras em geral interpretadas por artistas brasileiros bem como os suportes materiais ou arquivos digitais que os contenham, salvo na etapa de replicação industrial de mídias ópticas de leitura a laser. (CF88, Art 150, VI, e)</summary>
    FonogramasVideofonogramas = 5
}