using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly GLMSContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestsController(GLMSContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client) //  including client so we can show names
                .ToListAsync();

            return View(requests);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create(int? contractId)
        {
            // Showing client names instead of service levels
            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "Id",
                "Client.Name",
                contractId
            );

            var request = new ServiceRequest();
            if (contractId.HasValue)
            {
                request.ContractId = contractId.Value;
            }

            return View(request);
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            // Loading the parent contract first
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Invalid contract selected.");
            }
            else if (contract.Status == "Expired" || contract.Status == "On Hold")
            {
                ModelState.AddModelError("", "Cannot create a ServiceRequest for an Expired or On Hold contract.");
            }

            if (ModelState.IsValid)
            {
                // Fetching USD→ZAR rate
                var rate = await GetUsdToZarRateAsync();

                // Converting entered USD cost to ZAR before saving
                serviceRequest.Cost = serviceRequest.Cost * rate;

                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();

                // Redirecting back to Contract Details
                return RedirectToAction("Details", "Contracts", new { id = serviceRequest.ContractId });
            }

            // Repopulating dropdown with client names if validation fails
            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "Id",
                "Client.Name",
                serviceRequest.ContractId
            );

            return View(serviceRequest);
        }

        // Helper method to fetch USD→ZAR rate
        private async Task<decimal> GetUsdToZarRateAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");

            var json = System.Text.Json.JsonDocument.Parse(response);
            return json.RootElement.GetProperty("rates").GetProperty("ZAR").GetDecimal();
        }
    }
}
