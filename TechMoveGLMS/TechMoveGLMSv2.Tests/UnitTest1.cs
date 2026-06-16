using System.Net;
using System.Threading.Tasks;
using Xunit;
using TechMoveGLMS.Api;   // reference your API project
using System.Net.Http.Json;

namespace TechMoveGLMSv2.Tests   // matches your test project name
{
    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UnitTest1(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Contracts API
        [Fact]
        public async Task GetContracts_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/contracts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contracts = await response.Content.ReadFromJsonAsync<object>();
            Assert.NotNull(contracts);
        }

        [Fact]
        public async Task PostContract_WithValidClient_ReturnsCreated()
        {
            // Use the existing ClickPharmacy client (id = 17)
            var contract = new
            {
                clientId = 17,   //client id that exists in swagger ui
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddMonths(1),
                status = "Active",
                serviceLevel = "Standard"
            };

            var response = await _client.PostAsJsonAsync("/api/contracts", contract);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // lients API
        [Fact]
        public async Task GetClients_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/clients");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //ServiceRequests API
        [Fact]
        public async Task GetServiceRequests_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/servicerequests");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
