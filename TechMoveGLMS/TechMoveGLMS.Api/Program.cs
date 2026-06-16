using Microsoft.EntityFrameworkCore;           // EF Core
using Microsoft.OpenApi.Models;                // Swagger support
using TechMoveGLMS.Api.Data;                   // DbContext + DbInitializer
using System.Text.Json.Serialization;          // Needed for IgnoreCycles

namespace TechMoveGLMS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Swagger/OpenAPI setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TechMoveGLMS API",
                    Version = "v1"
                });
            });

            // Register DbContext
            builder.Services.AddDbContext<GLMSContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechMoveGLMS API v1");
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // ✅ Seed database on startup
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GLMSContext>();
                DbInitializer.Seed(context);
            }

            app.Run();
        }
    }
}
