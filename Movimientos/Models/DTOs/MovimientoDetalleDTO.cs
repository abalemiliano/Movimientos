
namespace Movimientos.Models.DTOs;

public record MovimientoDetalleDTO
{
    public int Id { get; set; }
    public string Detalle { get; set; } = string.Empty;
    public decimal Importe { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public int IdRubro { get; set; }
    public string RubroNombre { get; set; } = string.Empty;
    public bool EsIngreso { get; set; } = false;
    public string RubroColor { get; set; } = string.Empty;

    public string ImporteFormateado => $"{(EsIngreso ? "+" : "-")} $ {Importe:N2}";
}
