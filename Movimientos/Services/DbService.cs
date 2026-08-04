using Movimientos.Data;
using Movimientos.Models;
using Movimientos.Models.DTOs;
using Movimientos.Services.Interfaces;
using SQLite;

namespace Movimientos.Services;

public class DbService(DbContext dbContext) : IDbService
{
    private readonly DbContext _dbContext = dbContext;

    private async Task<SQLiteAsyncConnection> Init()
    {
        return await _dbContext.Init();
    }

    public async Task<Movimiento> ConsultarMovimientoAsync(int id)
    {
        var database = await Init();
        var movimiento = await database!.Table<Movimiento>().Where(m => m.Id == id).FirstOrDefaultAsync();
        return movimiento;
    }

    public async Task<int> EliminarMovimientoAsync(Movimiento movimiento)
    {
        var database = await Init();
        return await database!.DeleteAsync(movimiento);
    }

    /// <summary>
    /// Consulta para obtener totales de los ingresos y egresos de un mes.
    /// </summary>
    public async Task<(decimal TotalIngresos, decimal TotalEgresos)> ObtenerBalanceMensualAsync(int mes, int anio)
    {
        var database = await Init();

        var rubros = await database!.Table<Rubro>().ToListAsync();

        var fechaInicio = new DateTime(anio, mes, 1);
        var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

        var movimientos = await database.Table<Movimiento>()
            .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
            .ToListAsync();

        var totalIngresos = (from m in movimientos
                             join r in rubros on m.IdRubro equals r.Id
                             where r.EsIngreso
                             select m.Importe).Sum();

        var totalEgresos = (from m in movimientos
                            join r in rubros on m.IdRubro equals r.Id
                            where !r.EsIngreso
                            select m.Importe).Sum();

        return (totalIngresos, totalEgresos);
    }

    public async Task<int> RegistrarMovimientoAsync(Movimiento movimiento)
    {
        var database = await Init();
        if (movimiento.Id != 0)
            return await database!.UpdateAsync(movimiento);
        else
            return await database!.InsertAsync(movimiento);
    }

    /// <summary>
    /// Consulta para armar el grafico de gastos por rubro por mes y año.
    /// </summary>
    public async Task<List<ResumenRubroDTO>> TraerGastosPorRubroAsync(int mes, int anio)
    {
        var database = await Init();

        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1).AddDays(-1);

        var rubros = await database!.Table<Rubro>().ToListAsync();
        var movimientosMes = await database!.Table<Movimiento>()
            .Where(m => m.Fecha >= inicioMes && m.Fecha <= finMes)
            .ToListAsync();
        var egresosMes = (from m in movimientosMes
                       join r in rubros on m.IdRubro equals r.Id
                       where r.EsIngreso == false
                       select new
                       {
                           m.Importe
                       });
        var totalEgresosMes = egresosMes.Sum(e => e.Importe);

        if (totalEgresosMes == 0)
            return [];

        var resultado = (from m in movimientosMes
                         join r in rubros on m.IdRubro equals r.Id
                         where !r.EsIngreso // Filtramos solo Egresos
                         group m by new { r.Nombre, r.Color } into grupo
                         select new ResumenRubroDTO
                         {
                             Nombre = grupo.Key.Nombre,
                             Color = grupo.Key.Color,
                             Total = grupo.Sum(x => x.Importe)
                         })
                         .OrderByDescending(x => x.Total)
                         .ToList();
        foreach (var item in resultado)
        {
            item.Porcentaje = (double)Math.Round((item.Total / totalEgresosMes) * 100, 2);
        }

        return resultado;
    }

    public async Task<List<Movimiento>> TraerMovimientosAsync()
    {
        var database = await Init();
        return await database!.Table<Movimiento>().ToListAsync();
    }

    public async Task<List<MovimientoDetalleDTO>> TraerMovimientosUltimosAsync(int mes, int anio)
    {
        var database = await Init();

        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1).AddDays(-1);

        var rubros = await database!.Table<Rubro>().ToListAsync();
        var movimientosMes = await database!.Table<Movimiento>()
            .Where(m => m.Fecha >= inicioMes && m.Fecha <= finMes)
            .ToListAsync();

        var movimientos = (from m in movimientosMes
                           join r in rubros on m.IdRubro equals r.Id
                           select new MovimientoDetalleDTO
                           {
                               Id = m.Id,
                               Detalle = m.Detalle,
                               Importe = m.Importe,
                               Fecha = m.Fecha,
                               RubroNombre = r.Nombre,
                               EsIngreso = r.EsIngreso,
                               RubroColor = r.Color
                           })
                            .OrderByDescending(m => m.Fecha)
                            .Take(5)
                            .ToList();

        return movimientos;
    }

    public async Task<List<MovimientoDetalleDTO>> TraerMovimientosPaginadosAsync(int mes, int anio, int page, int limit, int? idRubro)
    {
        var database = await Init();

        var inicioMes = new DateTime(anio, mes, 1);
        var finMes = inicioMes.AddMonths(1).AddDays(-1);

        var rubros = await database!.Table<Rubro>().ToListAsync();

        var movimientosQuery = database!.Table<Movimiento>()
            .Where(m => m.Fecha >= inicioMes && m.Fecha <= finMes);

        if (idRubro.HasValue)
            movimientosQuery = movimientosQuery.Where(m => m.IdRubro == idRubro);

        var movimientosMes = await movimientosQuery
            .OrderByDescending(m => m.Fecha)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        var movimientos = movimientosMes.Select(m =>
        {
            var rubro = rubros.FirstOrDefault(r => r.Id == m.IdRubro);
            return new MovimientoDetalleDTO
            {
                Id = m.Id,
                Detalle = m.Detalle,
                Importe = m.Importe,
                Fecha = m.Fecha,
                IdRubro = m.IdRubro,
                RubroNombre = rubro?.Nombre ?? "Sin Rubro",
                EsIngreso = rubro?.EsIngreso ?? false,
                RubroColor = rubro?.Color ?? string.Empty
            };
        }).ToList();

        return movimientos;
    }

    public async Task<List<Rubro>> TraerRubrosAsync()
    {
        var database = await Init();
        return await database!.Table<Rubro>().ToListAsync();
    }

    public async Task<int> CrearRubroAsync(Rubro rubro)
    {
        var database = await Init();
        if (rubro.Id != 0)
            return await database!.UpdateAsync(rubro);
        else
            return await database!.InsertAsync(rubro);
    }
}
