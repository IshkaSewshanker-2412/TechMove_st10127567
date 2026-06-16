using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class ClientsController : Controller
    {
        private readonly GLMSContext _context;

        public ClientsController(GLMSContext context)
        {
            _context = context;
        }

        // Listing all clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // Showing create form
        public IActionResult Create()
        {
            return View();
        }

        // Handling form submission
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Debugging: showing validation errors if save fails
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(client);
        }
    }
}
