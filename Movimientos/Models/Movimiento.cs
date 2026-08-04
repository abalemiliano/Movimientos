
using SQLite;

namespace Movimientos.Models;

[Table("Movimientos")]
public class Movimiento
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int IdRubro { get; set; }
    public string Detalle { get; set; } = string.Empty;
    public decimal Importe { get; set; }
    [Indexed]
    public DateTime Fecha { get; set; } = DateTime.Today;
    public DateTime FechaTrx { get; set; } = DateTime.Now;
}
