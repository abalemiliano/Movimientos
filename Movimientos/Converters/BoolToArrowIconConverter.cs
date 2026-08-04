
using System.Globalization;

namespace Movimientos.Converters;

public class BoolToArrowIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            return "\ue5db";
        }

        if (value is false)
        {
            return "\ue5d8";
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
