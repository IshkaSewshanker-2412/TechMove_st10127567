using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Models
{
    public class Contract
    {
        //declaring variables for the contract class
        public int Id { get; set; }
        
        [Required]
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string ServiceLevel { get; set; }
        public string? SignedAgreementPath  { get; set; } 
        public ICollection<ServiceRequest>? ServiceRequests { get; set; }

    }
}
