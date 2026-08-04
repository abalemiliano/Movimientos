using Movimientos.ViewModels;

namespace Movimientos.Views;

public partial class RubrosPage : ContentPage
{
	public RubrosPage(RubrosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}