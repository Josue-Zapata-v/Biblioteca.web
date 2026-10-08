using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio : IPrestamoRepositorio
{
    private readonly string _connectionString;

    public PrestamoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CadenaConexion")
                            ?? configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Cadena de conexión no encontrada en appsettings.json.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<PrestamoReporte>> ObtenerReporteAsync(DateTime? fechaDesde, DateTime? fechaHasta)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@FechaDesde", fechaDesde, DbType.Date);
        parametros.Add("@FechaHasta", fechaHasta, DbType.Date);

        return await connection.QueryAsync<PrestamoReporte>(
            "sp_ReportePrestamos",
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }
}
