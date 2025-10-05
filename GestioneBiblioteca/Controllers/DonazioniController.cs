using Microsoft.AspNetCore.Mvc;
using GestioneBiblioteca.Models;
using Stripe;
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
            {
                // Mostra gli errori per debug
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(donazione);
            }

            try
            {
                // Salva donazione preliminare nel DB
                donazione.DataDonazione = DateTime.Now;
                donazione.PagamentoRiuscito = false;
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante la creazione della donazione: {ex.Message}");
                return View(donazione);
            }
        }

        // POST: Conferma pagamento
        [HttpPost]
        public IActionResult ConfermaPagamento([FromBody] PaymentData data)
        {
            try
            {
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
            return View();
        }
    }

    public class PaymentData
    {
        public string PaymentIntentId { get; set; }
    }
}