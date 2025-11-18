using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------
// 1?? Add services
// -------------------------------------------------------------

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
});

// -------------------------------------------------------------
// 3?? Authorization policies (optional future use)
// -------------------------------------------------------------

// -------------------------------------------------------------
// 4?? Build the app
// -------------------------------------------------------------
var app = builder.Build();


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

//app.UseAuthentication(); 
//app.UseAuthorization();

// -------------------------------------------------------------
// 6?? Routing configuration
// -------------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
    //pattern: "{controller=HR}/{action=Dashboard}/{id?}");

app.Run();
