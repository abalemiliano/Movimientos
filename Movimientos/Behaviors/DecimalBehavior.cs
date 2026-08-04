
using System.Globalization;
using System.Text;

namespace Movimientos.Behaviors;

public class DecimalBehavior : Behavior<Entry>
{
    private bool cambiando;
    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);
        entry.TextChanged += Entry_TextChanged;
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= Entry_TextChanged;
        base.OnDetachingFrom(entry);
    }

    private void Entry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (cambiando)
            return;

        if (sender is not Entry entry)
            return;

        cambiando = true;

        var separadorDecimal = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        var texto = e.NewTextValue ?? "";
        texto = texto.Replace(".", separadorDecimal).Replace(",", separadorDecimal);

        var sb = new StringBuilder();
        bool hayDecimal = false;
        int cantDecimal = 0;

        foreach (var c in texto)
        {
            if (char.IsDigit(c))
            {
                if (!hayDecimal)
                    sb.Append(c);
                else if (cantDecimal < 2)
                {
                    sb.Append(c);
                    cantDecimal++;
                }
            }
            else if (c.ToString() == separadorDecimal && !hayDecimal)
            {
                hayDecimal = true;
                sb.Append(separadorDecimal);
            }
        }

        var resultado = sb.ToString();
        if (entry.Text != resultado)
            entry.Text = resultado;

        cambiando = false;
    }
}
