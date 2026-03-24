using Microsoft.AspNetCore.Mvc;
using Agromercado.AppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Agromercado.AppMVC.Controllers
{
    public class RoleController : BaseController
    {
        private readonly AgroMercadoSprintDbContext _context;

        public RoleController(AgroMercadoSprintDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public IActionResult Index()
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            var roles = _context.Roles.ToList();
            return View(roles);
        }

        // CREAR GET
        public IActionResult Create()
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // EDITAR GET
        public IActionResult Edit(int id)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            var role = _context.Roles.Find(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        // EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Role role)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(role);

            var roleDb = _context.Roles.Find(id);
            if (roleDb == null)
                return NotFound();

            roleDb.Nombre = role.Nombre;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DETALLE
        public IActionResult Details(int id)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            var role = _context.Roles
                .Include(r => r.Empleados)
                .FirstOrDefault(r => r.Id == id);

            if (role == null)
                return NotFound();

            return View(role);
        }

        // GET: Role/Delete/5
        public IActionResult Delete(int id)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            var role = _context.Roles
                .Include(r => r.Empleados)
                .FirstOrDefault(r => r.Id == id);

            if (role == null)
                return NotFound();

            return View(role);
        }

        // POST: Role/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!TieneAcceso(1))
                return RedirectToAction("Index", "Home");

            var role = _context.Roles
                .Include(r => r.Empleados)
                .FirstOrDefault(r => r.Id == id);

            if (role == null)
                return NotFound();

            // ⚠ Validación importante
            if (role.Empleados.Any())
            {
                ModelState.AddModelError("", "No se puede eliminar el rol porque tiene empleados asignados.");
                return View("Delete", role);
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}