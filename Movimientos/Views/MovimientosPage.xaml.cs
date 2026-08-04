using Movimientos.ViewModels;

namespace Movimientos.Views;

public partial class MovimientosPage : ContentPage
{
	public MovimientosPage(MovimientosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}