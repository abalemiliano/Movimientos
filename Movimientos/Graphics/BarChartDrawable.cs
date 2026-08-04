
using Movimientos.Models.DTOs;

namespace Movimientos.Graphics;

public class BarChartDrawable : IDrawable
{
    public List<BalanceDTO> Items{ get; set; } = [];

    public float margen = 15;
    public float anchoBarra = 60;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (Items.Count == 0)
            return;

        int cantidadBarras = Items.Count;

        float altoGrafico = dirtyRect.Height - margen * 2;

        float baseY = dirtyRect.Height - margen;

        float espacio = dirtyRect.Width / (cantidadBarras + 1);

        decimal max = Items.Max(i => i.Importe);
        if (max <= 0)
            return;

        foreach (var item in Items)
        {
            float espacioItem = espacio * item.NroItem;
            float alturaItem = (float)(item.Importe / max) * altoGrafico;
            float yEjeItem = baseY - alturaItem;
            float xEjeItem = espacioItem - (anchoBarra / 2);

            canvas.FillColor = item.Color;
            canvas.FillRectangle(xEjeItem, yEjeItem, anchoBarra, alturaItem);

            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12;
            canvas.DrawString($"$ {item.Importe:N2}", espacioItem, yEjeItem - 5, HorizontalAlignment.Center);
        }

        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 1;
        canvas.DrawLine(0, baseY, dirtyRect.Width, baseY);
    }
}
