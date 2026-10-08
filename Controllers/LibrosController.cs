using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly ILibroRepositorio _libroRepositorio;

    public LibrosController(ILibroRepositorio libroRepositorio)
    {
        _libroRepositorio = libroRepositorio;
    }

    // GET: /Libros o /Libros/Index?buscarTitulo=...
    public async Task<IActionResult> Index(string? buscarTitulo)
    {
        IEnumerable<Libro> libros;
        if (!string.IsNullOrWhiteSpace(buscarTitulo))
        {
            libros = await _libroRepositorio.BuscarPorTituloAsync(buscarTitulo);
        }
        else
        {
            libros = await _libroRepositorio.ListarActivosAsync();
        }

        ViewData["Title"] = "Catálogo de Libros";
        ViewData["BuscarTitulo"] = buscarTitulo;
        return View(libros);
    }

    // GET: /Libros/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Detalle del Libro";
        return View(libro);
    }

    // GET: /Libros/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Registrar Nuevo Libro";
        await CargarAutoresDropDownListAsync();
        return View(new Libro());
    }

    // POST: /Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Registrar Nuevo Libro";
            await CargarAutoresDropDownListAsync(libro.AutorId);
            return View(libro);
        }

        await _libroRepositorio.InsertarAsync(libro);
        TempData["Mensaje"] = $"El libro '{libro.Titulo}' ha sido registrado exitosamente.";
        TempData["TipoMensaje"] = "success";

        return RedirectToAction(nameof(Index));
    }

    // GET: /Libros/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Editar Libro";
        await CargarAutoresDropDownListAsync(libro.AutorId);
        return View(libro);
    }

    // POST: /Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.LibroId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Editar Libro";
            await CargarAutoresDropDownListAsync(libro.AutorId);
            return View(libro);
        }

        var exito = await _libroRepositorio.ActualizarAsync(libro);
        if (!exito)
        {
            TempData["Mensaje"] = "No fue posible actualizar el libro.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction(nameof(Index));
        }

        TempData["Mensaje"] = $"El libro '{libro.Titulo}' ha sido actualizado exitosamente.";
        TempData["TipoMensaje"] = "success";

        return RedirectToAction(nameof(Index));
    }

    // GET: /Libros/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Eliminar Libro";
        return View(libro);
    }

    // POST: /Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, int? libroId)
    {
        int targetId = libroId ?? id;
        var exito = await _libroRepositorio.EliminarLogicoAsync(targetId);
        if (exito)
        {
            TempData["Mensaje"] = "El libro ha sido retirado del catálogo correctamente.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo realizar la eliminación del libro.";
            TempData["TipoMensaje"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresDropDownListAsync(object? seleccionado = null)
    {
        var autores = await _libroRepositorio.ListarAutoresActivosAsync();
        ViewBag.Autores = new SelectList(autores, "AutorId", "Nombre", seleccionado);
    }
}
