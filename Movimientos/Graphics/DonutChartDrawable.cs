
using Movimientos.Models.DTOs;

namespace Movimientos.Graphics;

public class DonutChartDrawable : IDrawable
{
    public List<ResumenRubroDTO> Items { get; set; } = [];

    public float strokeSize = 35;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (Items.Count <= 0)
            return;

        var total = Items.Sum(i => i.Total);
        if (total <= 0)
            return;

        float size = Math.Min(dirtyRect.Width, dirtyRect.Height) - strokeSize;
        float x = (dirtyRect.Width - size) / 2;
        float y = (dirtyRect.Height - size) / 2;

        float start = -90;

        foreach (var item in Items)
        {
            float traslado = (float)(item.Total / (decimal)total) * 360f;

            canvas.StrokeColor = Color.FromArgb(item.Color);
            canvas.StrokeSize = strokeSize;
            canvas.DrawArc(x, y, size, size, start, start + traslado, false, false);

            start += traslado;
        }

        canvas.FillColor = Colors.Black;
        canvas.FontSize = 20;
        canvas.DrawString(
            $"Total\n$ {total:N2}",
            dirtyRect,
            HorizontalAlignment.Center,
            VerticalAlignment.Center);
    }
}
