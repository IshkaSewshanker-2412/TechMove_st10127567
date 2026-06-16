namespace TechMoveGLMS.Api.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }

        // Navigation property
        public Contract? Contract { get; set; }
    }
}
