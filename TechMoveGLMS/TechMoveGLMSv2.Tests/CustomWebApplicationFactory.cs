using Microsoft.AspNetCore.Mvc.Testing;
using TechMoveGLMS.Api; 

namespace TechMoveGLMSv2.Tests
{
    // This spins up the API project for integration tests
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
    }
}
   