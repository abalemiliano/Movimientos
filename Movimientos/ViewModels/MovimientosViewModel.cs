
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Movimientos.Models;
using Movimientos.Models.DTOs;
using Movimientos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Movimientos.ViewModels;

public partial class MovimientosViewModel(IDbService dbService) : BaseViewModel
{
    private readonly IDbService _dbService = dbService;

    private int paginaActual = 1;
    private const int paginaLimite = 15;
    private bool hayMasDatos = true;
    private bool isInitialized = false;

    [ObservableProperty]
    private string? textEmptyView;

    private Mes? mesSeleccionado;
    public Mes? MesSeleccionado
    {
        get => mesSeleccionado;
        set
        {
            if (SetProperty(ref mesSeleccionado, value) && value != null)
            {
                if (isInitialized && !IsBusy)
                    _ = PickerChangedAsync();
            } 
        }
    }

    private int anioSeleccionado;
    public int AnioSeleccionado
    {
        get => anioSeleccionado;
        set
        {
            if (SetProperty(ref anioSeleccionado, value))
            {
                if (isInitialized && !IsBusy)
                    _ = PickerChangedAsync();
            }
        }
    }

    private Rubro? rubroSeleccionado;
    public Rubro? RubroSeleccionado
    {
        get => rubroSeleccionado;
        set
        {
            if (SetProperty(ref rubroSeleccionado, value))
            {
                if (isInitialized && !IsBusy)
                    _ = PickerChangedAsync();
            }
        }
    }

    public ObservableCollection<Mes> Meses { get; } = [];
    public ObservableCollection<int> Anios { get; } = [];
    public ObservableCollection<Rubro> Rubros { get; } = [];
    public ObservableCollection<MovimientoDetalleDTO> Movimientos { get; } = [];

    private async Task InicializarVariables()
    {
        isInitialized = false;
        CargarMeses();
        CargarAnios();
        await CargarRubros();
        paginaActual = 1;
        hayMasDatos = true;
        MesSeleccionado = Meses.FirstOrDefault(m => m.Numero == DateTime.Today.Month);
        AnioSeleccionado = DateTime.Today.Year;
        RubroSeleccionado = null;
        TextEmptyView = string.Empty;
        Movimientos.Clear();
    }

    private void CargarMeses()
    {
        if (Meses.Count == 0)
        {
            var cultura = new System.Globalization.CultureInfo("es-AR");

            for (int i = 1; i <= 12; i++)
            {
                Meses.Add(new Mes
                {
                    Numero = i,
                    Nombre = cultura.DateTimeFormat.GetMonthName(i).ToUpper()
                });
            }
        }
    }

    private void CargarAnios()
    {
        if (Anios.Count == 0)
        {
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
            {
                Anios.Add(i);
            }
        }
    }

    private async Task CargarRubros()
    {
        if (Rubros.Count == 0)
        {
            var rubros = await _dbService.TraerRubrosAsync();
            foreach (var rubro in rubros)
            {
                Rubros.Add(rubro);
            }
        }
    }

    private async Task CargarMovimientosAsync()
    {
        if (IsBusy || !hayMasDatos)
            return;

        try
        {
            IsBusy = true;
            var movimientos = await _dbService.TraerMovimientosPaginadosAsync(MesSeleccionado!.Numero, AnioSeleccionado, paginaActual, paginaLimite, rubroSeleccionado?.Id);
            if (movimientos is not null && movimientos.Count > 0)
            {
                foreach (var movimiento in movimientos)
                {
                    Movimientos.Add(movimiento);
                }
                paginaActual++;
            }
            else
            {
                hayMasDatos = false;
                TextEmptyView = "No hay información disponible.";
            }
        }
        catch (Exception ex)
        {
            TextEmptyView = $"Error al cargar los movimientos: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            isInitialized = true;
        }
    }

    private async Task PickerChangedAsync()
    {
        Movimientos.Clear();
        paginaActual = 1;
        hayMasDatos = true;
        await CargarMovimientosAsync();
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await InicializarVariables();
        await CargarMovimientosAsync();
    }

    [RelayCommand]
    private async Task CargarMasMovimientosAsync() => await CargarMovimientosAsync();
}

public class Mes
{
    public int Numero { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
