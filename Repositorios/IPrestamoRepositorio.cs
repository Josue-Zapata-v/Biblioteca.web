using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface IPrestamoRepositorio
{
    Task<IEnumerable<PrestamoReporte>> ObtenerReporteAsync(DateTime? fechaDesde, DateTime? fechaHasta);
}
