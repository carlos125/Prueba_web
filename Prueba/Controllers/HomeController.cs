using Microsoft.AspNetCore.Mvc;
using Prueba.Models;
using System.Diagnostics;

namespace Prueba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Usuarios()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Aquí puedes agregar la lógica para validar el usuario
            // Por ejemplo, verificar el email y la contraseña en una base de datos

            if (model.Email == "usuario@example.com" && model.Password == "password123")
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Credenciales incorrectas" });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
