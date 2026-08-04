using Movimientos.ViewModels;

namespace Movimientos.Views;

public partial class MovimientoPage : ContentPage
{
	public MovimientoPage(MovimientoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}