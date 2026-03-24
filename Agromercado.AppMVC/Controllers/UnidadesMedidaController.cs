using Microsoft.AspNetCore.Mvc;
using Agromercado.AppMVC.Models;
using System.Linq;

namespace Agromercado.AppMVC.Controllers
{
    public class UnidadesMedidaController : BaseController
    {
        private readonly AgroMercadoSprintDbContext _context;

        public UnidadesMedidaController(AgroMercadoSprintDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public IActionResult Index()
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            var lista = _context.UnidadMedida.ToList();
            return View(lista);
        }

        // DETALLES
        public IActionResult Details(int id)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            var unidad = _context.UnidadMedida.Find(id);
            if (unidad == null)
                return NotFound();

            return View(unidad);
        }

        // CREAR GET
        public IActionResult Create()
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnidadMedidum unidad)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(unidad);

            _context.UnidadMedida.Add(unidad);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDITAR GET
        public IActionResult Edit(int id)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            var unidad = _context.UnidadMedida.Find(id);
            if (unidad == null)
                return NotFound();

            return View(unidad);
        }

        // EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UnidadMedidum unidad)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(unidad);

            var unidadDb = _context.UnidadMedida.Find(id);
            if (unidadDb == null)
                return NotFound();

            unidadDb.Nombre = unidad.Nombre;
            unidadDb.Abreviatura = unidad.Abreviatura;
            unidadDb.Tipo = unidad.Tipo;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR GET
        public IActionResult Delete(int id)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            var unidad = _context.UnidadMedida.Find(id);
            if (unidad == null)
                return NotFound();

            return View(unidad);
        }

        // ELIMINAR POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!TieneAcceso(1, 6, 8))
                return RedirectToAction("Index", "Home");

            var unidad = _context.UnidadMedida.Find(id);
            if (unidad != null)
            {
                _context.UnidadMedida.Remove(unidad);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}