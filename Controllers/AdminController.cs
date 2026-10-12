using System.Text;
using CarreraCFE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarreraCFE.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AdminController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        private bool Autorizado()
        {
            var usuario = _config["Admin:Usuario"];
            var clave = _config["Admin:Clave"];
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave)) return false;

            var header = Request.Headers.Authorization.ToString();
            if (!header.StartsWith("Basic ")) return false;
            try
            {
                var datos = Encoding.UTF8
                    .GetString(Convert.FromBase64String(header.Substring(6)))
                    .Split(':', 2);
                return datos.Length == 2 && datos[0] == usuario && datos[1] == clave;
            }
            catch { return false; }
        }

        private IActionResult PedirLogin()
        {
            Response.Headers.WWWAuthenticate = "Basic realm=\"Admin\"";
            return Unauthorized();
        }

        public async Task<IActionResult> Index()
        {
            if (!Autorizado()) return PedirLogin();
            var lista = await _db.Corredores.OrderBy(c => c.Id).ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Descargar()
        {
            if (!Autorizado()) return PedirLogin();
            var lista = await _db.Corredores.OrderBy(c => c.Id).ToListAsync();

            string Esc(string? s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";

            var sb = new StringBuilder();
            sb.AppendLine("Folio,Nombre,Edad,Genero,Categoria,Correo,Telefono,EmpleadoCFE,RPE,Talla,FechaRegistro");
            foreach (var c in lista)
            {
                sb.AppendLine(string.Join(",",
                    "CFE-" + c.Id.ToString("D4"), Esc(c.Nombre), c.Edad, Esc(c.Genero),
                    Esc(c.CategoriaEdad), Esc(c.Correo), Esc(c.Telefono),
                    c.EsEmpleadoCFE ? "Si" : "No", Esc(c.RPE), Esc(c.TallaPlayera),
                    c.FechaRegistro.AddHours(-6).ToString("yyyy-MM-dd HH:mm")));
            }

            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
            return File(bytes, "text/csv", "inscritos.csv");
        }
    }
}