using System.Diagnostics;
using GestioneBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestioneBiblioteca.Controllers
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
            // ========== TEST SESSIONE - AGGIUNGI QUESTO BLOCCO ==========
            var name = HttpContext.Session.GetString(SessionKeys.SessionKeyName);
            var age = HttpContext.Session.GetInt32(SessionKeys.SessionKeyAge);

            if (string.IsNullOrEmpty(name))
            {
                ViewBag.SessionMessage = " Sessione vuota! Vai su /Donazioni/Create per impostarla.";
                ViewBag.HasSession = false;
            }
            else
            {
                ViewBag.SessionMessage = $"Sessione attiva: {name}, {age} anni";
                ViewBag.HasSession = true;
                ViewBag.UserName = name;
                ViewBag.UserAge = age;
            }
            // ========== FINE TEST SESSIONE ==========

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Libri()
        {
            return View();
        }

       
        public IActionResult Prestiti()
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
