namespace emoreiraExamen.Views;

public partial class vLogin : ContentPage
{
    private readonly string[,] credenciales =
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public vLogin() => InitializeComponent();

    private async void IniciarSesion_Clicked(object sender, EventArgs e)
    {
        string usuario = (txtUsuario.Text ?? string.Empty).Trim();
        string contrasena = txtContrasena.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlertAsync("Validación", "Ingrese usuario y contraseña.", "Aceptar");
            return;
        }

        bool acceso = false;
        for (int i = 0; i < credenciales.GetLength(0); i++)
        {
            if (usuario == credenciales[i, 0] && contrasena == credenciales[i, 1])
            {
                acceso = true;
                break;
            }
        }

        if (!acceso)
        {
            await DisplayAlertAsync("Datos incorrectos", "Usuario o contraseña incorrectos.", "Aceptar");
            txtContrasena.Text = string.Empty;
            return;
        }

        txtContrasena.Text = string.Empty;
        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private async void AcercaDe_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }

    public void LimpiarCredenciales()
    {
        txtUsuario.Text = string.Empty;
        txtContrasena.Text = string.Empty;
    }
}
