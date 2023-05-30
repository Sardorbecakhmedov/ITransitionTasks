using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Task_4_Authentication_Authorization.Helper;
using Task_4_Core.ApplicationDbContext;
using Task_4_Core.Contracts;
using Task_4_Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.LoginPath = CookieAuthenticationDefaults.LoginPath;
        config.LogoutPath = CookieAuthenticationDefaults.LogoutPath;
        config.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
    });

builder.Services.AddAuthorization();


builder.Services.AddDbContext<AppDbContext>(config =>
{
    string connectionPath = builder.Configuration.GetConnectionString("ConnectionSqlServer")!;
    config.UseSqlServer(connectionPath);

    //config.UseInMemoryDatabase("Memory");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
