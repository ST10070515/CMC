using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------
// 1?? Add services
// -------------------------------------------------------------

// Add MVC controllers and views
builder.Services.AddControllersWithViews();

// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity setup with custom User and Role (int keys)
/*builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // true if you want email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();*/

// Configure Identity password and lockout rules
/*builder.Services.Configure<IdentityOptions>(options =>
{
    // Password requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

// -------------------------------------------------------------
// 2?? Cookie Authentication Setup
// -------------------------------------------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirect if not logged in
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if no access
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});*/

// -------------------------------------------------------------
// 3?? Authorization policies (optional future use)
// -------------------------------------------------------------
/*builder.Services.AddAuthorization(options =>
{
    // Example role-based policies (optional)
    options.AddPolicy("RequireLecturerRole", policy => policy.RequireRole("Lecturer"));
    options.AddPolicy("RequireHRRole", policy => policy.RequireRole("HR"));
    options.AddPolicy("RequireAcademicManagerRole", policy => policy.RequireRole("AcademicManager"));
    options.AddPolicy("RequireProgrammeCoordinatorRole", policy => policy.RequireRole("ProgrammeCoordinator"));
});*/

// -------------------------------------------------------------
// 4?? Build the app
// -------------------------------------------------------------
var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        // Execute the async seeding method
        await IdentitySeedHelper.EnsureSeedData(serviceProvider);
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the identity database.");
    }
}
*/

// -------------------------------------------------------------
// 5?? Middleware pipeline
// -------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication(); // Must be before Authorization
//app.UseAuthorization();

// -------------------------------------------------------------
// 6?? Routing configuration
// -------------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// -------------------------------------------------------------
// 7?? (Optional) Auto-seed roles and admin accounts at startup
// -------------------------------------------------------------
// Uncomment and adjust if you want roles automatically created on startup.
/*
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    string[] roles = { "Lecturer", "ProgrammeCoordinator", "AcademicManager", "HR" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new Role { Name = roleName });
    }
}
*/

app.Run();
