using System.Runtime.CompilerServices;
using FoolProof.Core;

[assembly: InternalsVisibleTo("FoolProof.Core.Tests.E2eTests")]

namespace FoolProof.Core.Tests.E2eTests.WebApp;

public static class Program
{
    public static void Main(string[] args)
        => BuildApp(args).Run();

    internal static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddFoolProof();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}