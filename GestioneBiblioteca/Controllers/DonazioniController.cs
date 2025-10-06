using Microsoft.AspNetCore.Mvc;
using GestioneBiblioteca.Models;
using Stripe;
using TuoProgetto.Data;
using Microsoft.EntityFrameworkCore;

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
            // Calcola il totale delle donazioni riuscite
            var totaleDonazioni = _context.Donazioni
                .Where(d => d.PagamentoRiuscito)
                .Sum(d => (decimal?)d.Importo) ?? 0;

            // Salva in ViewBag per mostrarlo in tutte le pagine
            ViewBag.TotaleDonazioni = totaleDonazioni;

            return View();
        }

        // POST: Donazioni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Donazione donazione)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(donazione);
            }

            try
            {
                // Verifica connessione database
                if (!_context.Database.CanConnect())
                {
                    ModelState.AddModelError("", "Impossibile connettersi al database");
                    return View(donazione);
                }

                // Salva donazione preliminare nel DB
                donazione.DataDonazione = DateTime.Now;
                donazione.PagamentoRiuscito = false;
                _context.Donazioni.Add(donazione);
                _context.SaveChanges();

                // Verifica chiave Stripe
                var stripeKey = _config["Stripe:SecretKey"];
                if (string.IsNullOrEmpty(stripeKey))
                {
                    ModelState.AddModelError("", "Chiave Stripe non configurata");
                    return View(donazione);
                }

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
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Dettagli: {ex.InnerException.Message}");
                }
                return View(donazione);
            }
        }

        // POST: Conferma pagamento
        [HttpPost]
        public IActionResult ConfermaPagamento([FromBody] PaymentData data)
        {
            try
            {
                if (string.IsNullOrEmpty(data?.PaymentIntentId))
                {
                    return BadRequest(new { success = false, message = "PaymentIntentId mancante" });
                }

                var service = new PaymentIntentService();
                var intent = service.Get(data.PaymentIntentId);

                var donazione = _context.Donazioni.FirstOrDefault(d => d.PaymentIntentId == data.PaymentIntentId);

                if (donazione != null && intent.Status == "succeeded")
                {
                    donazione.PagamentoRiuscito = true;
                    _context.SaveChanges();
                    return Ok(new { success = true });
                }

                return BadRequest(new { success = false, message = "Pagamento non riuscito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Success
        public IActionResult Success()
        {
            // Calcola il totale delle donazioni per mostrarlo nella pagina di successo
            var totaleDonazioni = _context.Donazioni
                .Where(d => d.PagamentoRiuscito)
                .Sum(d => (decimal?)d.Importo) ?? 0;

            ViewBag.TotaleDonazioni = totaleDonazioni;

            return View();
        }

        // API per ottenere il totale donazioni (per mostrarlo in navbar)
        [HttpGet]
        public IActionResult GetTotaleDonazioni()
        {
            var totale = _context.Donazioni
                .Where(d => d.PagamentoRiuscito)
                .Sum(d => (decimal?)d.Importo) ?? 0;

            return Json(new { totale = totale });
        }
    }

    public class PaymentData
    {
        public string PaymentIntentId { get; set; }
    }
}