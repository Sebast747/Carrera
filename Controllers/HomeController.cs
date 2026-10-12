using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarreraCFE.Data;
using CarreraCFE.Models;

namespace CarreraCFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                corredor.FechaRegistro = DateTime.UtcNow;
                _context.Corredores.Add(corredor);
                await _context.SaveChangesAsync();

                // Redirige a una vista de confirmación mostrando los datos del registro
                return View("Exito", corredor);
            }

            return View("Index", corredor);
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