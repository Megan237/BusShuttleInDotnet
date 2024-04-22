using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebMvc.Service;
using WebMvc.Database;
using Microsoft.Extensions.Options;
namespace WebMvc;
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<BusDb>(Options => Options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<BusServiceInterface, BusService>();
        builder.Services.AddScoped<LoopServiceInterface, LoopService>();
        builder.Services.AddScoped<DriverServiceInterface, DriverService>();
        builder.Services.AddScoped<EntryServiceInterface, EntryService>();
        builder.Services.AddScoped<RouteServiceInterface, RouteService>();
        builder.Services.AddScoped<StopServiceInterface, StopService>();
        builder.Services.AddScoped<UserServiceInterface, UserService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "Cookies";
                })
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/home/index";
                    options.LogoutPath = "/home/Logout";
                    options.ReturnUrlParameter = "returnURL";
                    options.Cookie.HttpOnly = true;
                });

        builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
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
    }
}