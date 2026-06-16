using TechMoveGLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace TechMoveGLMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Registering HttpClient for API calls
            builder.Services.AddHttpClient();

            // ✅ Register ApiService so controllers can use it
            builder.Services.AddScoped<ApiService>();

            // ❌ In Task 2, the MVC project should NOT use DbContext directly.
            // Commented out to prevent direct SQL access.
            /*
            builder.Services.AddDbContext<GLMSContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            */

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();   // ✅ Correct way to serve CSS/JS/images
            app.UseRouting();
            app.UseAuthorization();

            // ✅ Standard MVC routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
