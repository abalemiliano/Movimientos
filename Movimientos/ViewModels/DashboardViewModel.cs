
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Movimientos.Models;
using Movimientos.Models.DTOs;
using Movimientos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Movimientos.ViewModels;

public partial class DashboardViewModel(IDbService dbService) : BaseViewModel
{
    private readonly IDbService _dbService = dbService;

    private ObservableCollection<BalanceDTO> balanceItems = [];
    private ObservableCollection<ResumenRubroDTO> items = [];
    private bool isInitialized = false;

    [ObservableProperty]
    private bool barChartOff;
    [ObservableProperty]
    private bool donutChartOff;
    [ObservableProperty]
    private decimal totalIngresos;
    [ObservableProperty]
    private decimal totalEgresos;
    [ObservableProperty]
    private decimal saldo;

    private Mes? mesSeleccionado;
    public Mes? MesSeleccionado
    {
        get => mesSeleccionado;
        set
        {
            if (SetProperty(ref mesSeleccionado, value) && value != null)
            {
                if (isInitialized && !IsBusy)
                    _ = CargarDashboardAsync();
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
                    _ = CargarDashboardAsync();
            }
        }
    }

    [ObservableProperty]
    public ObservableCollection<BalanceDTO> balance = [];
    [ObservableProperty]
    public ObservableCollection<ResumenRubroDTO> gastosPorRubro = [];

    public ObservableCollection<Mes> Meses { get; } = [];
    public ObservableCollection<int> Anios { get; } = [];
    public ObservableCollection<Movimiento> UltimosMovimientos { get; } = [];

    private void InicializarVariables()
    {
        isInitialized = false;
        CargarMeses();
        CargarAnios();
        MesSeleccionado = Meses.FirstOrDefault(m => m.Numero == DateTime.Today.Month);
        AnioSeleccionado = DateTime.Today.Year;
        BarChartOff = false;
        DonutChartOff = false;
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

    private async Task CargarDashboardAsync()
    {
        if (IsBusy)
            return;

        try
        {
            BarChartOff = false;
            DonutChartOff = false;
            IsBusy = true;
            int itemBalance = 0;

            var (ingresos, egresos) = await _dbService.ObtenerBalanceMensualAsync(MesSeleccionado!.Numero, AnioSeleccionado);
            TotalIngresos = ingresos;
            TotalEgresos = egresos;
            Saldo = TotalIngresos - TotalEgresos;

            if (TotalIngresos == 0 && TotalEgresos == 0)
                BarChartOff = true;

            Balance = [];
            balanceItems = [];
            itemBalance++;
            balanceItems.Add(new BalanceDTO() { NroItem = itemBalance, Nombre = "Ingresos", Importe = TotalIngresos, Color = Colors.Green });
            itemBalance++;
            balanceItems.Add(new BalanceDTO() { NroItem = itemBalance, Nombre = "Egresos", Importe = TotalEgresos, Color = Colors.Red });
            Balance = balanceItems;

            var gastos = await _dbService.TraerGastosPorRubroAsync(MesSeleccionado!.Numero, AnioSeleccionado);
            GastosPorRubro = [];

            if (gastos is not null && gastos.Count > 0)
            {
                decimal totalGral = gastos.Sum(g => g.Total);
                if (totalGral > 0)
                {
                    decimal umbralPorcentaje = 0.05m;
                    var principales = gastos.Where(g => (g.Total / totalGral) >= umbralPorcentaje)
                        .OrderByDescending(g => g.Total)
                        .ToList();

                    var secundarios = gastos.Where(g => (g.Total / totalGral) < umbralPorcentaje).ToList();

                    items = [];
                    foreach (var gasto in principales)
                    {
                        items.Add(gasto);
                    }

                    if (secundarios.Count != 0)
                    {
                        decimal totalOtros = secundarios.Sum(m => m.Total);
                        items.Add(new ResumenRubroDTO
                        {
                            Nombre = "Otros",
                            Total = totalOtros,
                            Color = "#607D8B",
                            Porcentaje = (double)Math.Round((totalOtros / TotalEgresos) * 100, 2)
                        });
                    }

                    GastosPorRubro = items;
                }
            }
            else
                DonutChartOff = true;
        }
        catch
        {
            
        }
        finally
        {
            IsBusy = false;
            isInitialized = true;
        }
    }

    [RelayCommand]
    private async Task Appearing()
    {
        InicializarVariables();
        await CargarDashboardAsync();
    }
}
