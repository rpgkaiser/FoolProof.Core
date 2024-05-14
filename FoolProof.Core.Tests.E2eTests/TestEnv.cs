using FoolProof.Core.Tests.E2eTests.WebApp;

namespace FoolProof.Core.Tests.E2eTests
{
    internal static class TestEnv
    {
        public static string? WebAppUrl { get; set; }

        private static CustomWebAppFactory? Factory { get; set; }

        public static void StartApp(TestContext testContext)
        {
            var port = int.TryParse(testContext.Properties["WebAppPort"] + "", out var p) ? p : 8080;
            Factory = new CustomWebAppFactory(port);
            WebAppUrl = Factory?.ServerAddress;
        }

        public static void StopApp() 
            => Factory?.AppHost?.StopAsync();
    }

    [TestClass]
    public class InitTestEnv
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext) => TestEnv.StartApp(testContext);

        [AssemblyCleanup]
        public static void AssemblyCleanup() => TestEnv.StopApp();
    }
}
