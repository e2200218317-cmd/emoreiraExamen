using System.Globalization;
namespace emoreiraExamen.Models;

public class Socio
{
    public const decimal MembresiaAnual = 1200m;
    public const decimal RecargoPorCuota = 24m; // 2 % de 1200, EN CADA cuota.
    public const decimal CostoCasilleroPorCuota = 10m;
    public const int NumeroCuotas = 5;

    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public int Edad { get; init; }
    public DateTime FechaInicio { get; init; }
    public string Plan { get; init; } = string.Empty;
    public bool Casillero { get; init; }
    public decimal MontoInicial { get; init; }

    public decimal PagoMensual => decimal.Round(
        (MembresiaAnual - MontoInicial) / NumeroCuotas
        + RecargoPorCuota + (Casillero ? CostoCasilleroPorCuota : 0m),
        2, MidpointRounding.AwayFromZero);

    public decimal PagoTotal => MontoInicial + NumeroCuotas * PagoMensual;

    public static string Moneda(decimal monto) =>
        monto.ToString("C2", CultureInfo.GetCultureInfo("es-EC"));
}
