
namespace Movimientos.Models.DTOs;

public record BalanceDTO
{
    public int NroItem { get; set; }
    public string? Nombre { get; set; }
    public decimal Importe { get; set; }
    public Color? Color { get; set; }
}
