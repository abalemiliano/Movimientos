
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Movimientos.Models;
using Movimientos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Movimientos.ViewModels;

public partial class MovimientoViewModel(IDbService dbService) : BaseViewModel
{
    private readonly IDbService _dbService = dbService;

    [ObservableProperty]
    private Rubro? rubroSeleccionado;

    [ObservableProperty]
    private string detalle = string.Empty;

    [ObservableProperty]
    private string importe = string.Empty;

    [ObservableProperty]
    private DateTime fecha = DateTime.Today;

    [ObservableProperty]
    private string textGuardar = "Guardar";

    public ObservableCollection<Rubro> Rubros { get; } = [];

    private async Task CargarRubrosAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var listaRubros = await _dbService.TraerRubrosAsync();

            Rubros.Clear();
            foreach (var rubro in listaRubros)
            {
                Rubros.Add(rubro);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudieron cargar los rubros: {ex.Message}", "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await CargarRubrosAsync();
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (RubroSeleccionado == null)
        {
            await Shell.Current.DisplayAlertAsync("Validación", "Debes seleccionar un rubro.", "Aceptar");
            return;
        }

        if (decimal.TryParse(Importe, out var valor))
        {
            if (valor <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Validación", "El importe debe ser mayor a cero.", "Aceptar");
                return;
            }
        }

        try
        {
            TextGuardar = string.Empty;
            IsBusy = true;

            var nuevoMovimiento = new Movimiento
            {
                IdRubro = RubroSeleccionado.Id,
                Detalle = Detalle.Trim(),
                Importe = valor,
                Fecha = Fecha.Date
            };

            await _dbService.RegistrarMovimientoAsync(nuevoMovimiento);

            await Shell.Current.DisplayAlertAsync("Éxito", "Movimiento registrado correctamente.", "Aceptar");
            await Shell.Current.GoToAsync("//DashboardPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo guardar el movimiento: {ex.Message}", "Aceptar");
        }
        finally
        {
            IsBusy = false;
            TextGuardar = "Guardar";
        }
    }
}
