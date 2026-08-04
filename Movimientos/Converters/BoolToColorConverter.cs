
using System.Globalization;

namespace Movimientos.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            return Colors.Green;
        }

        if (value is false)
        {
            return Colors.Red;
        }

        return Colors.Green;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
