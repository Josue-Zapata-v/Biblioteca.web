using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio : ISocioRepositorio
{
    private readonly string _connectionString;

    public SocioRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CadenaConexion")
                            ?? configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Cadena de conexión no encontrada en appsettings.json.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Socio>> ListarActivosAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Socio>(
            "sp_ListarSociosActivos",
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> InsertarAsync(Socio socio)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@DNI", socio.DNI, DbType.String);
        parametros.Add("@Nombre", socio.Nombre, DbType.String);
        parametros.Add("@Email", socio.Email, DbType.String);

        var id = await connection.ExecuteScalarAsync<int>(
            "sp_InsertarSocio",
            parametros,
            commandType: CommandType.StoredProcedure
        );
        return id;
    }

    public async Task<bool> ExisteDniAsync(string dni)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@DNI", dni, DbType.String);

        var conteo = await connection.ExecuteScalarAsync<int>(
            "sp_ExisteSocioPorDNI",
            parametros,
            commandType: CommandType.StoredProcedure
        );
        return conteo > 0;
    }
}
