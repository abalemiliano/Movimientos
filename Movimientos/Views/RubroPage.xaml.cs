using Maui.ColorPicker;
using Movimientos.ViewModels;

namespace Movimientos.Views;

public partial class RubroPage : ContentPage
{
	public RubroPage(RubroViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void ColorPicker_PickedColorChanged(object sender, Maui.ColorPicker.PickedColorChangedEventArgs e)
    {
        if (sender is ColorPicker picker)
        {
            ColorPreview.Color = picker.PickedColor;
            ((RubroViewModel)BindingContext).Color = picker.PickedColor.ToArgbHex();
        }
    }
}