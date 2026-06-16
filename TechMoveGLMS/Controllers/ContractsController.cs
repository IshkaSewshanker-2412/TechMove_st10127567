using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly GLMSContext _context;

        public ContractsController(GLMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
            return View(contracts);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            // Using SelectList instead of raw list
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // POST: Contracts/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            // Custom validation: EndDate must be after StartDate
            if (contract.EndDate <= contract.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date must be after Start Date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulating dropdown if validation fails
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            return View(contract);
        }

        

        // GET: Contracts/Details
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Search
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate, string status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status.Contains(status) || c.Client.Name.Contains(status));
            }

            var results = await query.ToListAsync();
            return View("Index", results); // Reusing the Index view to show filtered results
        }

        // POST: Contracts/UploadAgreement
        [HttpPost]
        public async Task<IActionResult> UploadAgreement(int id, IFormFile file)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            if (file != null && file.Length > 0)
            {
                // Only allowing PDF
                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("", "Only PDF files are allowed.");
                    return RedirectToAction("Details", new { id });
                }

                // Saving file to wwwroot/uploads
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

                // Saving path in contract
                contract.SignedAgreementPath = fileName;
                _context.Update(contract);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }

        // GET: Contracts/DownloadAgreement
        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, contract.SignedAgreementPath);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", contract.SignedAgreementPath);
        }

    }
}
