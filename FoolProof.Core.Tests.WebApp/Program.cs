namespace FoolProof.Core.Tests.E2eTests.WebApp;

public class Program
{
    public static void Main(string[] args)
        => BuildApp(WebApplication.CreateBuilder(args)).Run();

    internal static WebApplication BuildApp(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddFoolProof();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}