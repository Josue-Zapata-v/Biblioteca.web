using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface ILibroRepositorio
{
    Task<IEnumerable<Libro>> ListarActivosAsync();
    Task<IEnumerable<Libro>> BuscarPorTituloAsync(string? titulo);
    Task<Libro?> ObtenerPorIdAsync(int libroId);
    Task<int> InsertarAsync(Libro libro);
    Task<bool> ActualizarAsync(Libro libro);
    Task<bool> EliminarLogicoAsync(int libroId);
    Task<IEnumerable<Autor>> ListarAutoresActivosAsync();
}
