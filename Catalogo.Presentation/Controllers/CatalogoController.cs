using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // AGREGADO: Necesario para usar .Where()

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;

        public CatalogoController(ItemService service)
        {
            _service = service;
        }

        // MODIFICADO: Se agregó searchString para el buscador
        public IActionResult Index(string? genero, string? searchString)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            // AGREGADO: Lógica del buscador
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Titulo.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;
            ViewBag.SearchString = searchString; // AGREGADO: Para mantener el texto en el input

            return View(items);
        }

        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}