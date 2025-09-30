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

        public static int Wait4MsgTimeout { get; private set; } = 30000;

        public static int CallServerRetryCount { get; private set; } = 1;

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

            if (int.TryParse(testContext.Properties["Wait4MsgTimeout"] + "", out var waitTimeout))
                Wait4MsgTimeout = waitTimeout;

            if (int.TryParse(testContext.Properties["CallServerRetryCount"] + "", out var retryCount))
                CallServerRetryCount = retryCount;

            Trace.WriteLine($"Executing E2E tests using {(UseJQuery ?? true ? "jquery.validation" : "aspnet-client-validation")} as the client-side validation library.");
        }

        [AssemblyCleanup]
        public static void Cleanup()
            => Factory?.AppHost?.StopAsync();
    }
}