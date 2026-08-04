
namespace Movimientos.Models.DTOs;

public record ResumenRubroDTO
{
    public string? Nombre { get; set; }
    public decimal Total { get; set; }
    public string Color { get; set; } = "#E7E7E7";
    public double Porcentaje { get; set; }
}
