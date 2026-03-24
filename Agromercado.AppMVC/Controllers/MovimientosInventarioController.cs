using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agromercado.AppMVC.Models;

namespace Agromercado.AppMVC.Controllers
{
    public class MovimientosInventarioController : Controller
    {
        private readonly AgroMercadoSprintDbContext _context;

        public MovimientosInventarioController(AgroMercadoSprintDbContext context)
        {
            _context = context;
        }

        // ============================
        // LISTA DE MOVIMIENTOS
        // ============================
        public IActionResult Index()
        {
            var movimientos = _context.MovimientosInventarios
                .Include(m => m.Producto)
                .OrderByDescending(m => m.Fecha)
                .ToList();

            return View(movimientos);
        }

        // ============================
        // FORMULARIO ENTRADA INICIAL
        // ============================
        public IActionResult CrearEntradaInicial()
        {
            ViewBag.Productos = new SelectList(
                _context.Productos.Where(p => p.Activo == true),
                "Id",
                "Nombre"
            );

            return View();
        }

        // ============================
        // GUARDAR ENTRADA INICIAL
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearEntradaInicial(int productoId, int cantidad)
        {
            if (cantidad <= 0)
                ModelState.AddModelError("", "La cantidad debe ser mayor a 0");

            var existe = _context.MovimientosInventarios
                .Any(m => m.ProductoId == productoId && m.TipoMovimiento == "Entrada Inicial");

            if (existe)
                ModelState.AddModelError("", "Este producto ya tiene stock inicial.");

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = new SelectList(
                    _context.Productos.Where(p => p.Activo == true),
                    "Id",
                    "Nombre"
                );
                return View();
            }

            var producto = _context.Productos.Find(productoId);
            if (producto == null) return NotFound();

            producto.Stock = cantidad;

            var movimiento = new MovimientosInventario
            {
                ProductoId = productoId,
                TipoMovimiento = "Entrada Inicial",
                Cantidad = cantidad,
                Fecha = DateTime.Now
            };

            _context.MovimientosInventarios.Add(movimiento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // FORMULARIO ENTRADA NORMAL
        // ============================
        public IActionResult CrearEntrada()
        {
            ViewBag.Productos = new SelectList(
                _context.Productos.Where(p => p.Activo == true),
                "Id",
                "Nombre"
            );

            return View();
        }

        // ============================
        // GUARDAR ENTRADA NORMAL
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearEntrada(int productoId, int cantidad)
        {
            if (cantidad <= 0)
                ModelState.AddModelError("", "Cantidad inválida");

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = new SelectList(
                    _context.Productos.Where(p => p.Activo == true),
                    "Id",
                    "Nombre"
                );
                return View();
            }

            var producto = _context.Productos.Find(productoId);
            if (producto == null) return NotFound();

            producto.Stock = (producto.Stock ?? 0) + cantidad;

            var movimiento = new MovimientosInventario
            {
                ProductoId = productoId,
                TipoMovimiento = "Entrada",
                Cantidad = cantidad,
                Fecha = DateTime.Now
            };

            _context.MovimientosInventarios.Add(movimiento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // FORMULARIO SALIDA
        // ============================
        public IActionResult CrearSalida()
        {
            ViewBag.Productos = new SelectList(
                _context.Productos.Where(p => p.Activo == true),
                "Id",
                "Nombre"
            );

            return View();
        }

        // ============================
        // GUARDAR SALIDA
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearSalida(int productoId, int cantidad)
        {
            if (cantidad <= 0)
                ModelState.AddModelError("", "Cantidad inválida");

            var producto = _context.Productos.Find(productoId);
            if (producto == null) return NotFound();

            if ((producto.Stock ?? 0) < cantidad)
                ModelState.AddModelError("", "Stock insuficiente");

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = new SelectList(
                    _context.Productos.Where(p => p.Activo == true),
                    "Id",
                    "Nombre"
                );
                return View();
            }

            producto.Stock -= cantidad;

            var movimiento = new MovimientosInventario
            {
                ProductoId = productoId,
                TipoMovimiento = "Salida",
                Cantidad = cantidad,
                Fecha = DateTime.Now
            };

            _context.MovimientosInventarios.Add(movimiento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // DETALLE
        // ============================
        public IActionResult Details(int id)
        {
            var movimiento = _context.MovimientosInventarios
                .Include(m => m.Producto)
                .FirstOrDefault(m => m.Id == id);

            if (movimiento == null) return NotFound();

            return View(movimiento);
        }

        // ============================
        // CONFIRMAR ELIMINAR
        // ============================
        public IActionResult Delete(int id)
        {
            var movimiento = _context.MovimientosInventarios
                .Include(m => m.Producto)
                .FirstOrDefault(m => m.Id == id);

            if (movimiento == null) return NotFound();

            return View(movimiento);
        }

        // ============================
        // ELIMINAR
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movimiento = _context.MovimientosInventarios.Find(id);

            if (movimiento != null)
            {
                var producto = _context.Productos.Find(movimiento.ProductoId);

                // 🔥 Revertir stock según tipo
                if (producto != null)
                {
                    if (movimiento.TipoMovimiento == "Entrada" || movimiento.TipoMovimiento == "Entrada Inicial")
                        producto.Stock -= movimiento.Cantidad;

                    if (movimiento.TipoMovimiento == "Salida")
                        producto.Stock += movimiento.Cantidad;
                }

                _context.MovimientosInventarios.Remove(movimiento);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}