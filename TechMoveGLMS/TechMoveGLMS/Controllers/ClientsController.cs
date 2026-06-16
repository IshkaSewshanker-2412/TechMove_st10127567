using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ClientsController : Controller
    {
        // ❌ In Task 2, we no longer use GLMSContext directly.
        // private readonly GLMSContext _context;

        // ✅ Instead, inject ApiService to call the Web API.
        private readonly ApiService _apiService;

        public ClientsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // List all clients
        public async Task<IActionResult> Index()
        {
            // ❌ Old code:
            // return View(await _context.Clients.ToListAsync());

            // ✅ New code: call API
            var clients = await _apiService.GetClientsAsync();
            return View(clients);
        }

        // Show create form
        public IActionResult Create()
        {
            return View();
        }

        // Handle form submission
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                // ✅ New code: call API
                await _apiService.CreateClientAsync(client);
                return RedirectToAction("Index");
            }

            // Debug: show validation errors if save fails
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(client);
        }
    }
}
