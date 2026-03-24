using Agromercado.AppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Agromercado.AppMVC.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Usuario = usuario;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
