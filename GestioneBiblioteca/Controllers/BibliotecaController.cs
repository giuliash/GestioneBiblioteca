using Microsoft.AspNetCore.Mvc;

namespace GestioneBiblioteca.Controllers
{
    public class BibliotecaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
