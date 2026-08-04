
using CommunityToolkit.Mvvm.ComponentModel;

namespace Movimientos.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;
    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? mensaje;
}
