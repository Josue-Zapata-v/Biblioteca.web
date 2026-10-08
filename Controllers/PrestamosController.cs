using Microsoft.AspNetCore.Mvc;
using Biblioteca.Web.Repositorios;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly IPrestamoRepositorio _prestamoRepositorio;

    public PrestamosController(IPrestamoRepositorio prestamoRepositorio)
    {
        _prestamoRepositorio = prestamoRepositorio;
    }

    // GET: /Prestamos/Reporte?desde=yyyy-MM-dd&hasta=yyyy-MM-dd
    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var reporte = await _prestamoRepositorio.ObtenerReporteAsync(desde, hasta);

        ViewData["Title"] = "Reporte de Préstamos";
        ViewData["Desde"] = desde?.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd");

        return View(reporte);
    }
}
