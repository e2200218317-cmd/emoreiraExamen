namespace emoreiraExamen.Views;

public partial class vAcercaDe : ContentPage
{
    public vAcercaDe() => InitializeComponent();

    private async void Volver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
