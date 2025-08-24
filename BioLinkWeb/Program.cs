using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Data;
using BioLinkWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=biolink.db"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()                // UI bawaan Identity
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Redirect root → /steven
app.MapGet("/", context =>
{
    context.Response.Redirect("/steven");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Index}/{id?}",
    defaults: new { controller = "Dashboard" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Biolink}/{action=Index}/{username?}"
);

// Identity Razor Pages
app.MapRazorPages();

app.Run();
