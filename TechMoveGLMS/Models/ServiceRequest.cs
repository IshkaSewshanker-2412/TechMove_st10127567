using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Models
{
    public class ServiceRequest
    {
        //declaring properties for the ServiceRequest class
        public int Id { get; set; }
        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
