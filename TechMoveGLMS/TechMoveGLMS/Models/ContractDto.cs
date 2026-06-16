namespace TechMoveGLMS.Models
{
    public class ContractDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }   // ✅ add this
        public DateTime EndDate { get; set; }     // ✅ add this
        public string Status { get; set; }
        public string ServiceLevel { get; set; }
        public ClientDto Client { get; set; }
        public List<ServiceRequestDto> ServiceRequests { get; set; }
        public string SignedAgreementPath { get; set; }
    }


    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string Region { get; set; }
    }

    public class ServiceRequestDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
    }
}
