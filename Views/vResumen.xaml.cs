using System.Globalization;
using System.Text;
using emoreiraExamen.Models;

namespace emoreiraExamen.Views;

public partial class vResumen : ContentPage
{
    private readonly Socio _socio;
    private readonly string _usuarioConectado;

    public vResumen(Socio socio, string usuarioConectado)
    {
        InitializeComponent();
        _socio = socio;
        _usuarioConectado = usuarioConectado;

        lblUsuarioConectado.Text = $"Usuario conectado: {usuarioConectado}";
        lblNombre.Text = socio.Nombre;
        lblApellido.Text = socio.Apellido;
        lblEdad.Text = socio.Edad.ToString(CultureInfo.InvariantCulture);
        lblFecha.Text = socio.FechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        lblPlan.Text = socio.Plan;
        lblCasillero.Text = socio.Casillero ? "Sí" : "No";
        lblMontoInicial.Text = Socio.Moneda(socio.MontoInicial);
        lblPagoMensual.Text = Socio.Moneda(socio.PagoMensual);
        lblPagoTotal.Text = Socio.Moneda(socio.PagoTotal);
    }

    private async void Exportar_Clicked(object? sender, EventArgs e)
    {
        try
        {
            string contenido = $"""
                RESUMEN DEL SOCIO
                Usuario conectado: {_usuarioConectado}
                Nombre: {_socio.Nombre}
                Apellido: {_socio.Apellido}
                Edad: {lblEdad.Text}
                Fecha de inicio: {lblFecha.Text}
                Plan: {_socio.Plan}
                Casillero: {lblCasillero.Text}
                Monto inicial: {lblMontoInicial.Text}
                Pago mensual: {lblPagoMensual.Text}
                Pago total: {lblPagoTotal.Text}
                """;

            string ruta = Path.Combine(FileSystem.CacheDirectory, $"Registro_{Guid.NewGuid():N}.txt");
            await File.WriteAllTextAsync(ruta, contenido, Encoding.UTF8);
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Exportar registro del socio",
                File = new ShareFile(ruta, "text/plain")
            });
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Exportación", "No se pudo exportar el registro. Inténtelo de nuevo.", "Aceptar");
        }
    }

    private async void NuevoRegistro_Clicked(object? sender, EventArgs e)
    {
        var registro = Navigation.NavigationStack.OfType<vRegistro>().LastOrDefault();
        if (registro is not null)
        {
            registro.LimpiarFormulario();
            await Navigation.PopAsync();
        }
    }

    private async void CerrarSesion_Clicked(object? sender, EventArgs e)
    {
        var login = Navigation.NavigationStack.OfType<vLogin>().FirstOrDefault();
        login?.LimpiarCredenciales();
        await Navigation.PopToRootAsync();
    }
}
