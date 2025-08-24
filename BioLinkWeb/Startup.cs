using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Data;
using BioLinkWeb.Models; // <--- penting
using Microsoft.AspNetCore.Identity; // <--- penting

namespace BioLinkWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=biolink.db"));

            // ✅ Tambahkan Identity dengan ApplicationUser
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

            services.AddRazorPages(); // <-- tambahkan ini
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "dashboard",
                    pattern: "Dashboard/{action=Index}/{id?}",
                    defaults: new { controller = "Dashboard" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Biolink}/{action=Index}/{id?}");
            // ✅ tambahkan untuk Identity UI
            endpoints.MapRazorPages();
            });
        }
    }
}
