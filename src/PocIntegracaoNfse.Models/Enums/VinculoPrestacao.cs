namespace PocIntegracaoNfse.Models.Enums;

/// <summary>
/// Vínculo entre as partes no negócio
/// Baseado em TSVincPrest
/// </summary>
public enum VinculoPrestacao
{
    /// <summary>1 - Controlada</summary>
    Controlada = 1,

    /// <summary>2 - Controladora</summary>
    Controladora = 2,

    /// <summary>3 - Coligada</summary>
    Coligada = 3,

    /// <summary>4 - Matriz</summary>
    Matriz = 4,

    /// <summary>5 - Filial ou sucursal</summary>
    FilialSucursal = 5,

    /// <summary>6 - Outro vínculo</summary>
    OutroVinculo = 6
}