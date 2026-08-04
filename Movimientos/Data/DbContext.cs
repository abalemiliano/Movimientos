
using Bogus;
using Movimientos.Models;
using Movimientos.Services.Interfaces;
using SQLite;

namespace Movimientos.Data;

public class DbContext(IDbPathService dbPathService)
{
    private readonly IDbPathService _dbPathService = dbPathService;
    private SQLiteAsyncConnection? _database;

    public async Task<SQLiteAsyncConnection> Init()
    {
        if (_database is not null)
            return _database;

        var dbPath = _dbPathService.GetPath(Constants.DbConstants.DatabaseFileName);
        _database = new SQLiteAsyncConnection(dbPath, Constants.DbConstants.Flags);

        await _database.CreateTableAsync<Rubro>();
        await _database.CreateTableAsync<Movimiento>();

        //await SeedDataAsync();  // Si quiero hacer una carga de ejemplo o algunos datos iniciales.

        return _database;
    }

    private async Task SeedDataAsync()
    {
        var countRubros = await _database!.Table<Rubro>().CountAsync();
        if (countRubros == 0)
        {
            var rubrosIniciales = new List<Rubro>
            {
                new() { Nombre = "Sueldo", EsIngreso = true, Color = "#2ECC71" },
                new() { Nombre = "Comida", EsIngreso = false, Color = "#E74C3C" },
                new() { Nombre = "Servicios", EsIngreso = false, Color = "#3498DB" },
                new() { Nombre = "Impuestos", EsIngreso = false, Color = "#7A49BF"},
                new() { Nombre = "Combustible", EsIngreso = false, Color = "#F39C12" },
                new() { Nombre = "Ocio", EsIngreso = false, Color = "#9B59B6" }
            };

            await _database.InsertAllAsync(rubrosIniciales);
        }

        var countMovimientos = await _database!.Table<Movimiento>().CountAsync();
        if (countMovimientos == 0)
        {
            var rubros = await _database!.Table<Rubro>().ToListAsync();
            var rubrosIds = rubros.Select(r => r.Id).ToList();
            var movimientoGenerator = new MovimientoGenerator(rubrosIds);
            var movimientos = movimientoGenerator.Generate(20);

            await _database!.InsertAllAsync(movimientos);
        }
    }
}

public class MovimientoGenerator : Faker<Movimiento>
{
    private static readonly DateTime inicio = new(DateTime.Today.Year, DateTime.Today.Month - 1, 1);
    private static readonly DateTime fin = DateTime.Today;
    public MovimientoGenerator(List<int> rubrosIds)
    {
        UseSeed(42)
            .RuleFor(m => m.IdRubro, m => m.Random.ListItem(rubrosIds))
            .RuleFor(m => m.Detalle, m => m.Commerce.ProductName())
            .RuleFor(m => m.Importe, m => m.Random.Number(0, 100000))
            .RuleFor(m => m.Fecha, m => m.Date.Between(inicio, fin));
    }
}
