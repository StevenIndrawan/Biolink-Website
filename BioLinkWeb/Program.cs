using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Data;
using BioLinkWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=biolink.db"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

// ✅ Seed data async
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await SeedUsersAsync(userManager);
}

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

app.MapGet("/", context =>
{
    context.Response.Redirect("/Identity/Account/Login");
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

app.MapRazorPages();

app.Run();

// ✅ Method seed user
static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
{
    if (!userManager.Users.Any())
    {
        var user1 = new ApplicationUser
        {
            UserName = "steven123",
            Email = "steven@email.com",
            DisplayName = "Steven",
            Bio = "Fullstack dev",
            IsActive = true,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user1, "Password123!");

        var user2 = new ApplicationUser
        {
            UserName = "amanda_dev",
            Email = "amanda@email.com",
            DisplayName = "Amanda",
            Bio = "UI/UX Designer",
            IsActive = false,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user2, "Password123!");
    }
}
