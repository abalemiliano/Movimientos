
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Movimientos.Models;
using Movimientos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Movimientos.ViewModels;

public partial class RubrosViewModel(IDbService dbService) : BaseViewModel
{
    private readonly IDbService _dbService = dbService;

    [ObservableProperty]
    private string? textEmptyView;

    public ObservableCollection<Rubro> Rubros { get; } = [];

    private async Task CargarRubrosAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Rubros.Clear();
            var rubros = await _dbService.TraerRubrosAsync();
            if (rubros.Count > 0)
            {
                foreach (var rubro in rubros)
                {
                    Rubros.Add(rubro);
                }
            }
            else
            {
                TextEmptyView = "No hay información disponible.";
            }
        }
        catch (Exception ex)
        {
            TextEmptyView = $"Error al cargar los rubros: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Appearing() => await CargarRubrosAsync();
}
