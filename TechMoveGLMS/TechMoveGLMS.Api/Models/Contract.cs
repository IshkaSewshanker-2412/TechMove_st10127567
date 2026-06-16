using TechMoveGLMS.Models;

namespace TechMoveGLMS.Api.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string ServiceLevel { get; set; }

        // Navigation properties
        public Client? Client { get; set; }
        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}
