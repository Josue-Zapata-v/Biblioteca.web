using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio : ILibroRepositorio
{
    private readonly string _connectionString;

    public LibroRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CadenaConexion")
                            ?? configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Cadena de conexión no encontrada en appsettings.json.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Libro>> ListarActivosAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Libro>(
            "sp_ListarLibrosActivos",
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string? titulo)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@Titulo", titulo, DbType.String);

        return await connection.QueryAsync<Libro>(
            "sp_BuscarLibrosPorTitulo",
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Libro?> ObtenerPorIdAsync(int libroId)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@LibroId", libroId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<Libro>(
            "sp_ObtenerLibroPorId",
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> InsertarAsync(Libro libro)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@Titulo", libro.Titulo, DbType.String);
        parametros.Add("@ISBN", libro.ISBN, DbType.String);
        parametros.Add("@AutorId", libro.AutorId, DbType.Int32);
        parametros.Add("@Ejemplares", libro.Ejemplares, DbType.Int32);

        var id = await connection.ExecuteScalarAsync<int>(
            "sp_InsertarLibro",
            parametros,
            commandType: CommandType.StoredProcedure
        );
        return id;
    }

    public async Task<bool> ActualizarAsync(Libro libro)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@LibroId", libro.LibroId, DbType.Int32);
        parametros.Add("@Titulo", libro.Titulo, DbType.String);
        parametros.Add("@ISBN", libro.ISBN, DbType.String);
        parametros.Add("@AutorId", libro.AutorId, DbType.Int32);
        parametros.Add("@Ejemplares", libro.Ejemplares, DbType.Int32);

        var filasAfectadas = await connection.ExecuteAsync(
            "sp_ActualizarLibro",
            parametros,
            commandType: CommandType.StoredProcedure
        );
        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarLogicoAsync(int libroId)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("@LibroId", libroId, DbType.Int32);

        var filasAfectadas = await connection.ExecuteAsync(
            "sp_EliminarLibro",
            parametros,
            commandType: CommandType.StoredProcedure
        );
        return filasAfectadas > 0;
    }

    public async Task<IEnumerable<Autor>> ListarAutoresActivosAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Autor>(
            "sp_ListarAutoresActivos",
            commandType: CommandType.StoredProcedure
        );
    }
}
