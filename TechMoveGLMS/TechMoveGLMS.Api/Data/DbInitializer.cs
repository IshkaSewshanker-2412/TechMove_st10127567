using TechMoveGLMS.Api.Data;
using TechMoveGLMS.Api.Models;

namespace TechMoveGLMS.Api.Data
{
    public static class DbInitializer
    {
        public static void Seed(GLMSContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Clients.Any())
            {
                var client1 = new Client { Name = "ABC Corp", ContactDetails = "abc@example.com", Region = "Durban" };
                var client2 = new Client { Name = "XYZ Ltd", ContactDetails = "xyz@example.com", Region = "Cape Town" };

                context.Clients.AddRange(client1, client2);

                context.Contracts.Add(new Contract
                {
                    Status = "Active",
                    ServiceLevel = "Gold",
                    Client = client1,
                    ServiceRequests = new List<ServiceRequest>
                    {
                        new ServiceRequest { Description = "Network setup", Status = "Open", Cost = 5000 }
                    }
                });

                context.Contracts.Add(new Contract
                {
                    Status = "Pending",
                    ServiceLevel = "Silver",
                    Client = client2,
                    ServiceRequests = new List<ServiceRequest>
                    {
                        new ServiceRequest { Description = "Software installation", Status = "Closed", Cost = 3000 }
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
