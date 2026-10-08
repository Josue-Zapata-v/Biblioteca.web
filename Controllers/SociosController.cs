using Microsoft.AspNetCore.Mvc;
using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly ISocioRepositorio _socioRepositorio;

    public SociosController(ISocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    // GET: /Socios
    public async Task<IActionResult> Index()
    {
        var socios = await _socioRepositorio.ListarActivosAsync();
        ViewData["Title"] = "Listado de Socios";
        return View(socios);
    }

    // GET: /Socios/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Registrar Nuevo Socio";
        return View(new Socio());
    }

    // POST: /Socios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        // Validación de negocio: comprobar si el DNI ya existe
        if (!string.IsNullOrWhiteSpace(socio.DNI) && await _socioRepositorio.ExisteDniAsync(socio.DNI))
        {
            ModelState.AddModelError("DNI", "El DNI ya se encuentra registrado en el sistema.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Registrar Nuevo Socio";
            return View(socio);
        }

        await _socioRepositorio.InsertarAsync(socio);
        TempData["Mensaje"] = $"El socio '{socio.Nombre}' ha sido registrado exitosamente.";
        TempData["TipoMensaje"] = "success";

        return RedirectToAction(nameof(Index));
    }
}
