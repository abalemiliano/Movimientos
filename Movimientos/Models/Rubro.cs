
using SQLite;

namespace Movimientos.Models;

[Table("Rubros")]
public class Rubro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EsIngreso { get; set; } = false;
    public string Color { get; set; } = "#E7E7E7";
}
