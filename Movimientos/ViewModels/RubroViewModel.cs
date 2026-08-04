
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Movimientos.Models;
using Movimientos.Services.Interfaces;
using System.Diagnostics;

namespace Movimientos.ViewModels;

public partial class RubroViewModel(IDbService dbService) : BaseViewModel
{
    private readonly IDbService _dbService = dbService;

    [ObservableProperty]
    private string? nombre;

    [ObservableProperty]
    private string? detalle;

    [ObservableProperty]
    private bool esIngreso;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private string textGuardar = "Guardar";

    [RelayCommand]
    private async Task Appearing()
    {
        Nombre = string.Empty;
        EsIngreso = true;
        Color = string.Empty;
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            await Shell.Current.DisplayAlertAsync("Validación", "El nombre del rubro no puede quedar vacío.", "Aceptar");
            return;
        }

        try
        {
            TextGuardar = string.Empty;
            IsBusy = true;

            Rubro rubro = new()
            {
                Nombre = Nombre,
                Descripcion = Detalle,
                EsIngreso = EsIngreso,
                Color = Color ?? "#E7E7E7"
            };

            await _dbService.CrearRubroAsync(rubro);

            await Shell.Current.DisplayAlertAsync("Éxito", "Rubro creado correctamente.", "Aceptar");
            await Shell.Current.GoToAsync("//DashboardPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo guardar el rubro: {ex.Message}", "Aceptar");
        }
        finally
        {
            IsBusy = false;
            TextGuardar = "Guardar";
        }
    }
}
