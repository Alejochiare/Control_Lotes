using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Control_de_Lotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string ciudad)
        {
            // Aquí podés implementar la lógica que necesites al recibir el nombre de una ciudad
            ViewBag.Mensaje = $"Seleccionaste la ciudad: {ciudad}";
            return View();
        }
    }
}
