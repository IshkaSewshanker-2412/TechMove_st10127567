using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Api.Data;      // ✅ DbContext from API
using TechMoveGLMS.Api.Models;    // ✅ Models from API

namespace TechMoveGLMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly GLMSContext _context;

        public ContractsController(GLMSContext context)
        {
            _context = context;
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .AsNoTracking()   // ✅ avoids EF tracking conflicts
                .ToListAsync();
        }

        // GET: api/contracts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .AsNoTracking()   // ✅ ensures EF doesn’t skip records
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        // POST: api/contracts
        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            // Return the created contract with proper route
            return CreatedAtAction(nameof(GetContract), new { id = contract.Id }, contract);
        }

        // PATCH: api/contracts/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            contract.Status = status;
            _context.Update(contract);
            await _context.SaveChangesAsync();

            return Ok(contract);
        }
    }
}
