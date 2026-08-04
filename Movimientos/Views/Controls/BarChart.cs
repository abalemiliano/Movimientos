
using Movimientos.Graphics;
using Movimientos.Models.DTOs;

namespace Movimientos.Views.Controls;

public class BarChart : GraphicsView
{
    public readonly BarChartDrawable drawable = new();

    public BarChart()
    {
        Drawable = drawable;
    }

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items),
        typeof(IEnumerable<BalanceDTO>),
        typeof(BarChart),
        default(IEnumerable<BalanceDTO>),
        propertyChanged: OnItemsChanged);

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var chart = (BarChart)bindable;

        chart.drawable.Items = (newValue as IEnumerable<BalanceDTO>)?.ToList()
                                ?? new List<BalanceDTO>();

        chart.Invalidate();
    }

    public IEnumerable<BalanceDTO>? Items
    {
        get => (IEnumerable<BalanceDTO>)GetValue(ItemsProperty);
        set
        {
            SetValue(ItemsProperty, value);
            OnPropertyChanged(nameof(Items));
        }
    }
}
