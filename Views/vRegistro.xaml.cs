using System.Globalization;
using System.Text.RegularExpressions;
using emoreiraExamen.Models;

namespace emoreiraExamen.Views;

public partial class vRegistro : ContentPage
{
    private readonly string _usuarioConectado;
    private Socio? _ultimoCalculo;
    private bool _limpiando;

    public vRegistro(string usuarioConectado)
    {
        InitializeComponent();
        _usuarioConectado = usuarioConectado;
        lblUsuarioConectado.Text = $"Usuario conectado: {_usuarioConectado}";
        LimpiarFormulario();
    }

    private void DatoModificado(object? sender, EventArgs e)
    {
        if (_limpiando) return;
        _ultimoCalculo = null;
        if (txtPagoMensual != null)
            txtPagoMensual.Text = string.Empty;
    }

    public void LimpiarFormulario()
    {
        _limpiando = true;
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtEdad.Text = string.Empty;
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
        pkPlan.SelectedIndex = -1;
        swCasillero.IsToggled = false;
        txtMontoInicial.Text = string.Empty;
        txtPagoMensual.Text = string.Empty;
        _ultimoCalculo = null;
        _limpiando = false;
    }

    private static bool NombreValido(string nombre)
    {
        return Regex.IsMatch(nombre, @"^\p{L}+(?:[ '\-]\p{L}+)*$");
    }

    private bool ValidarYCrearSocio(out Socio? socio, out string error)
    {
        socio = null;
        error = string.Empty;
        string nombre = (txtNombre.Text ?? string.Empty).Trim();
        string apellido = (txtApellido.Text ?? string.Empty).Trim();

        if (!NombreValido(nombre))
        {
            error = "Ingrese un nombre válido (solo letras).";
            return false;
        }
        if (!NombreValido(apellido))
        {
            error = "Ingrese un apellido válido (solo letras).";
            return false;
        }
        if (!int.TryParse((txtEdad.Text ?? string.Empty).Trim(), NumberStyles.None,
                CultureInfo.InvariantCulture, out int edad) || edad < 16)
        {
            error = "La edad debe ser un número entero mayor o igual a 16.";
            return false;
        }
        if (dpFechaInicio.Date is not DateTime fechaInicio)
        {
            error = "Seleccione una fecha de inicio.";
            return false;
        }
        if (fechaInicio.Date < DateTime.Today)
        {
            error = "La fecha de inicio no puede ser anterior a hoy.";
            return false;
        }
        if (pkPlan.SelectedIndex < 0)
        {
            error = "Seleccione un plan: Musculación, Cardio, Crossfit o Full.";
            return false;
        }

        string montoTexto = (txtMontoInicial.Text ?? string.Empty).Trim().Replace(',', '.');
        if (!decimal.TryParse(montoTexto, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out decimal inicial)
            || inicial < 120m || inicial > 600m)
        {
            error = "El monto inicial debe ser numérico y estar entre $120 y $600.";
            return false;
        }
        if (decimal.Round(inicial, 2) != inicial)
        {
            error = "El monto inicial admite máximo 2 decimales.";
            return false;
        }

        socio = new Socio
        {
            Nombre = nombre,
            Apellido = apellido,
            Edad = edad,
            FechaInicio = fechaInicio.Date,
            Plan = pkPlan.Items[pkPlan.SelectedIndex],
            Casillero = swCasillero.IsToggled,
            MontoInicial = inicial
        };
        return true;
    }

    private async void Calcular_Clicked(object? sender, EventArgs e)
    {
        if (!ValidarYCrearSocio(out Socio? socio, out string error))
        {
            await DisplayAlertAsync("Validación", error, "Aceptar");
            return;
        }
        _ultimoCalculo = socio;
        txtPagoMensual.Text = Socio.Moneda(socio!.PagoMensual);
    }

    private async void VerResumen_Clicked(object? sender, EventArgs e)
    {
        if (!ValidarYCrearSocio(out Socio? socio, out string error))
        {
            await DisplayAlertAsync("Validación", error, "Aceptar");
            return;
        }
        if (_ultimoCalculo is null)
        {
            await DisplayAlertAsync("Falta calcular", "Presione primero 'Calcular pago mensual'.", "Aceptar");
            return;
        }
        await Navigation.PushAsync(new vResumen(socio!, _usuarioConectado));
    }
}
