using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApiService _apiService;

        public ContractsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var contracts = await _apiService.GetContractsAsync();
            return View(contracts);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            var clients = await _apiService.GetClientsAsync();
            ViewBag.Clients = new SelectList(clients, "Id", "Name");
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractDto contract)
        {
            if (contract.EndDate <= contract.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date must be after Start Date.");
            }

            if (ModelState.IsValid)
            {
                await _apiService.CreateContractAsync(contract);
                return RedirectToAction(nameof(Index));
            }

            var clients = await _apiService.GetClientsAsync();
            ViewBag.Clients = new SelectList(clients, "Id", "Name");
            return View(contract);
        }

        // GET: Contracts/Details
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Search
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate, string status)
        {
            var contracts = await _apiService.GetContractsAsync();

            if (startDate.HasValue)
            {
                contracts = contracts.Where(c => c.StartDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                contracts = contracts.Where(c => c.EndDate <= endDate.Value).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                contracts = contracts.Where(c => c.Status.Contains(status) || c.Client?.Name.Contains(status) == true).ToList();
            }

            return View("Index", contracts);
        }

        // ---------------- PDF Upload/Download ----------------
        [HttpPost]
        public async Task<IActionResult> UploadAgreement(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a PDF file.";
                return RedirectToAction("Details", new { id });
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                TempData["Error"] = "Only PDF files are allowed.";
                return RedirectToAction("Details", new { id });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            TempData["UploadedFile"] = fileName;
            TempData["Success"] = "PDF uploaded successfully.";

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var fileName = TempData["UploadedFile"] as string;
            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }
    }
}