using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GestioneBiblioteca.Controllers
{
    public class UploadController : Controller
    {
        // Estensioni consentite
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new()
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".gif", new List<byte[]>
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38 }
                }
            },
            { ".pdf", new List<byte[]>
                {
                    new byte[] { 0x25, 0x50, 0x44, 0x46 }
                }
            }
        };

        private const long MaxFileSize = 3 * 1024 * 1024; // 3MB
        private static readonly string[] AllowedExtensions = { ".jpeg", ".jpg", ".gif", ".pdf" };

        // GET: Upload/UploadFile
        public IActionResult UploadFile()
        {
            return View();
        }

        // POST: Upload/UploadFile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ViewBag.Message = "Nessun file selezionato.";
                ViewBag.MessageType = "warning";
                return View();
            }

            var uploadedFiles = new List<string>();
            var errors = new List<string>();
            long totalSize = 0;

            foreach (var file in files)
            {
                // Validazione 1: Verifica che il file non sia vuoto
                if (file.Length == 0)
                {
                    errors.Add($"{file.FileName}: File vuoto.");
                    continue;
                }

                // Validazione 2: Verifica dimensione massima (3MB)
                if (file.Length > MaxFileSize)
                {
                    errors.Add($"{file.FileName}: Dimensione superiore a 3MB ({file.Length / 1024.0 / 1024.0:F2}MB).");
                    continue;
                }

                // Validazione 3: Verifica estensione file
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
                {
                    errors.Add($"{file.FileName}: Estensione non consentita. Ammesse: {string.Join(", ", AllowedExtensions)}");
                    continue;
                }

                // Validazione 4: Verifica firma file (file signature)
                using (var reader = new BinaryReader(file.OpenReadStream()))
                {
                    var signatures = _fileSignatures[extension];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    bool isValid = signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));

                    if (!isValid)
                    {
                        errors.Add($"{file.FileName}: Firma del file non valida. Il file potrebbe essere corrotto o non corrispondere all'estensione.");
                        continue;
                    }
                }

                // Salvataggio file
                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    // Crea la cartella se non esiste
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedFiles.Add(file.FileName);
                    totalSize += file.Length;
                }
                catch (Exception ex)
                {
                    errors.Add($"{file.FileName}: Errore durante il salvataggio - {ex.Message}");
                }
            }

            // Preparazione risultati
            ViewBag.UploadedCount = uploadedFiles.Count;
            ViewBag.TotalSize = $"{totalSize / 1024.0 / 1024.0:F2} MB";
            ViewBag.UploadedFiles = uploadedFiles;
            ViewBag.Errors = errors;

            if (uploadedFiles.Count > 0)
            {
                ViewBag.Message = $"Caricamento completato! {uploadedFiles.Count} file caricati con successo.";
                ViewBag.MessageType = "success";
            }
            else
            {
                ViewBag.Message = "Nessun file caricato. Controlla gli errori.";
                ViewBag.MessageType = "danger";
            }

            return View();
        }
    }
}
