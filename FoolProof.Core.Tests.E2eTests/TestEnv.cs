using FoolProof.Core.Tests.E2eTests.WebApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.E2eTests
{
    internal static class TestEnv
    {
        public static string? WebAppUrl { get; set; }

        private static CustomWebAppFactory? Factory { get; set; }

        public static void StartApp(TestContext testContext)
        {
            var port = int.TryParse(testContext.Properties["WebAppPort"] + "", out var p) ? p : 8080;
            if (bool.TryParse(testContext.Properties["StartWebApp"] + "", out var startWebApp) && startWebApp)
            {
                Factory = new CustomWebAppFactory(port);
                WebAppUrl = Factory?.ServerAddress;
            }
            else
                WebAppUrl = testContext.Properties["WebAppUrl"] as string;
        }

        public static void StopApp() 
            => Factory?.AppHost?.StopAsync();
    }

    [TestClass]
    public class InitTestEnv
    {
        public static bool StartWebApp { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext) => TestEnv.StartApp(testContext);

        [AssemblyCleanup]
        public static void AssemblyCleanup() => TestEnv.StopApp();
    }
}
