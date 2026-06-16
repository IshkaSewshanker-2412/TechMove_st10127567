using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Api.Data;      // ✅ DbContext from API
using TechMoveGLMS.Api.Models;    // ✅ Models from API

namespace TechMoveGLMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly GLMSContext _context;

        public ServiceRequestsController(GLMSContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetRequests()
        {
            return await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();
        }

        // ✅ NEW: GET: api/servicerequests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> PostRequest(ServiceRequest request)
        {
            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            // ✅ Point CreatedAtAction to GetRequest so Location header is correct
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }
    }
}
