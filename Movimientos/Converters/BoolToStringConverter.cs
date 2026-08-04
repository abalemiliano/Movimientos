
using System.Globalization;

namespace Movimientos.Converters;

public class BoolToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            return "Si";
        }

        if (value is false)
        {
            return "No";
        }

        return "Si";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
