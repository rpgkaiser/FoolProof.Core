using System.Diagnostics;
using FoolProof.Core.Tests.E2eTests.WebApp;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]

namespace FoolProof.Core.Tests.E2eTests
{
    [TestClass]
    public class TestEnv
    {
        public static string? WebAppUrl { get; set; }

        private static CustomWebAppFactory? Factory { get; set; }

        public static bool? UseJQuery { get; private set; } = true;

        [AssemblyInitialize]
        public static void Setup(TestContext testContext)
        {
            var port = int.TryParse(testContext.Properties["WebAppPort"] + "", out var p) ? p : 8080;
            if (bool.TryParse(testContext.Properties["StartWebApp"] + "", out var startWebApp) && startWebApp)
            {
                Factory = new CustomWebAppFactory(port);
                WebAppUrl = Factory?.ServerAddress;
            }
            else
                WebAppUrl = testContext.Properties["WebAppUrl"] as string;

            if (bool.TryParse(testContext.Properties["UseJQuery"] + "", out var useJQ))
                UseJQuery = useJQ;

            Trace.WriteLine($"Executing E2E tests using {(UseJQuery ?? true ? "jquery.validation" : "aspnet-client-validation")} as the client-side validation library.");
        }

        [AssemblyCleanup]
        public static void Cleanup()
            => Factory?.AppHost?.StopAsync();
    }
}
