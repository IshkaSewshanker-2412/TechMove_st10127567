using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApiService _apiService;

        public ServiceRequestsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests = await _apiService.GetServiceRequestsAsync();
            return View(requests);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create(int contractId)
        {
            // Load contracts from API
            var contracts = await _apiService.GetContractsAsync();

            // Populate dropdown with contract IDs + client names
            ViewBag.Contracts = new SelectList(contracts, "Id", "Client.Name", contractId);

            // Pre‑set ContractId so the hidden field in the view has the right value
            return View(new ServiceRequest { ContractId = contractId });
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            // Validate contract exists
            var contract = await _apiService.GetContractByIdAsync(serviceRequest.ContractId);
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
                // Convert USD → ZAR
                var rate = await GetUsdToZarRateAsync();
                serviceRequest.Cost = serviceRequest.Cost * rate;

                // Save via API
                await _apiService.CreateServiceRequestAsync(serviceRequest);

                // Redirect back to Contract Details
                return RedirectToAction("Details", "Contracts", new { id = serviceRequest.ContractId });
            }

            // Repopulate dropdown if validation fails
            var contracts = await _apiService.GetContractsAsync();
            ViewBag.Contracts = new SelectList(contracts, "Id", "Client.Name", serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // Helper method to fetch USD→ZAR rate
        private async Task<decimal> GetUsdToZarRateAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");
            var json = System.Text.Json.JsonDocument.Parse(response);
            return json.RootElement.GetProperty("rates").GetProperty("ZAR").GetDecimal();
        }
    }
}
