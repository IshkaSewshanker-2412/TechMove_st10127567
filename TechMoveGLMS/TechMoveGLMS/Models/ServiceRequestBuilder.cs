namespace TechMoveGLMS.Models
{
    public class ServiceRequestBuilder
    {
        private ServiceRequest _request = new ServiceRequest();

        public ServiceRequestBuilder SetContract(int contractId)
        {
            _request.ContractId = contractId;
            return this;
        }

        public ServiceRequestBuilder SetDescription(string description)
        {
            _request.Description = description;
            return this;
        }

        public ServiceRequestBuilder SetCost(decimal cost)
        {
            _request.Cost = cost;
            return this;
        }

        public ServiceRequestBuilder SetStatus(string status)
        {
            _request.Status = status;
            return this;
        }

        public ServiceRequest Build()
        {
            return _request;
        }
    }
}
