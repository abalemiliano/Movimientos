
using Movimientos.Graphics;
using Movimientos.Models.DTOs;

namespace Movimientos.Views.Controls;

public class DonutChart : GraphicsView
{
    private readonly DonutChartDrawable drawable = new();

    public DonutChart()
    {
        Drawable = drawable;
    }

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items),
        typeof(IEnumerable<ResumenRubroDTO>),
        typeof(DonutChart),
        default(IEnumerable<ResumenRubroDTO>),
        propertyChanged: OnItemsChanged);

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var chart = (DonutChart)bindable;

        chart.drawable.Items = (newValue as IEnumerable<ResumenRubroDTO>)?.ToList()
                                ?? new List<ResumenRubroDTO>();

        chart.Invalidate();
    }

    public IEnumerable<ResumenRubroDTO>? Items
    {
        get => (IEnumerable<ResumenRubroDTO>)GetValue(ItemsProperty);
        set
        {
            SetValue(ItemsProperty, value);
            OnPropertyChanged(nameof(Items));
        }
    }
}
