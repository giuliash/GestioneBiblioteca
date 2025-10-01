using Microsoft.AspNetCore.Mvc;
using GestioneBiblioteca.Data;    // DbContext
using GestioneBiblioteca.Models;  // Donazione, StripeSettings
using Stripe;                     // PaymentIntentService, StripeConfiguration
using Stripe.Checkout;            // opzionale se usi CheckoutSession
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using TuoProgetto.Data;

namespace GestioneBiblioteca.Controllers
{
    public class DonazioniController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public DonazioniController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Donazioni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donazioni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Donazione donazione)
        {
            if (!ModelState.IsValid)
                return View(donazione);

            // Salva donazione preliminare nel DB
            _context.Donazioni.Add(donazione);
            _context.SaveChanges();

            // Crea PaymentIntent Stripe
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(donazione.Importo * 100), // importo in centesimi
                Currency = "eur",
                Metadata = new Dictionary<string, string>
                {
                    {"DonazioneId", donazione.Id.ToString()},
                    {"Nome", donazione.Nome}
                }
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            donazione.PaymentIntentId = paymentIntent.Id;
            _context.SaveChanges();

            // Passa i dati alla view Checkout
            ViewBag.ClientSecret = paymentIntent.ClientSecret;
            ViewBag.PublishableKey = _config["Stripe:PublishableKey"];

            return View("Checkout", donazione);
        }

        // POST: Conferma pagamento
        [HttpPost]
        public IActionResult ConfermaPagamento([FromBody] dynamic data)
        {
            string paymentIntentId = data.paymentIntentId;
            var service = new PaymentIntentService();
            var intent = service.Get(paymentIntentId);

            var donazione = _context.Donazioni.FirstOrDefault(d => d.PaymentIntentId == paymentIntentId);
            if (donazione != null && intent.Status == "succeeded")
            {
                donazione.PagamentoRiuscito = true;
                _context.SaveChanges();
            }

            return Ok();
        }

        // GET: Success
        public IActionResult Success()
        {
            return View();
        }
    }
}
