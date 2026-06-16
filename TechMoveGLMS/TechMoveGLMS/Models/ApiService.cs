using System.Net.Http;
using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            // ✅ Read API base URL from configuration or environment
            var apiBaseUrl = config["ApiBaseUrl"];

            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                // Default to localhost for development outside Docker
                apiBaseUrl = "http://localhost:5000/";
            }

            // Ensure trailing slash
            if (!apiBaseUrl.EndsWith("/"))
                apiBaseUrl += "/";

            _httpClient.BaseAddress = new Uri(apiBaseUrl);
        }

        // ---------------- Clients ----------------
        public async Task<List<Client>> GetClientsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Client>>("api/clients") ?? new List<Client>();
        }

        public async Task<Client?> CreateClientAsync(Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("api/clients", client);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Client>();
        }

        // ---------------- Contracts ----------------
        public async Task<List<ContractDto>> GetContractsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ContractDto>>("api/contracts") ?? new List<ContractDto>();
        }

        public async Task<ContractDto?> CreateContractAsync(ContractDto contract)
        {
            var response = await _httpClient.PostAsJsonAsync("api/contracts", contract);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ContractDto>();
        }

        public async Task<ContractDto?> GetContractByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/contracts/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ContractDto>();
            }
            return null;
        }

        // ---------------- Service Requests ----------------
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests") ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?> CreateServiceRequestAsync(ServiceRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/servicerequests", request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<ServiceRequest>();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/servicerequests/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ServiceRequest>();
            }
            return null;
        }
    }
}
