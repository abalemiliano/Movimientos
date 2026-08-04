
using Movimientos.Models;
using Movimientos.Models.DTOs;

namespace Movimientos.Services.Interfaces;

public interface IDbService
{
    Task<List<Movimiento>> TraerMovimientosAsync();
    Task<List<MovimientoDetalleDTO>> TraerMovimientosUltimosAsync(int mes, int anio);
    Task<List<MovimientoDetalleDTO>> TraerMovimientosPaginadosAsync(int mes, int anio, int page, int limit, int? idRubro);
    Task<Movimiento> ConsultarMovimientoAsync(int id);
    Task<List<Rubro>> TraerRubrosAsync();

    Task<int> RegistrarMovimientoAsync(Movimiento movimiento);
    Task<int> EliminarMovimientoAsync(Movimiento movimiento);
    Task<int> CrearRubroAsync(Rubro rubro);

    Task<List<ResumenRubroDTO>> TraerGastosPorRubroAsync(int mes, int anio);
    Task<(decimal TotalIngresos, decimal TotalEgresos)> ObtenerBalanceMensualAsync(int mes, int anio);
}
