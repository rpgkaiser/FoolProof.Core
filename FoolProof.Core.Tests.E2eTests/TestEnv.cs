using FoolProof.Core.Tests.E2eTests.WebApp;
using Microsoft.AspNetCore.Builder;

namespace FoolProof.Core.Tests.E2eTests
{
    internal static class TestEnv
    {
        public static string Url => Application?.Urls?.First() ?? string.Empty;

        private static WebApplication? Application { get; set; }

        public static void StartApp()
        {
            Application = Program.BuildApp([]);
            Application.RunAsync();
        }

        public static void StopApp() => Application?.StopAsync().Wait();
    }

    [TestClass]
    public class InitTestEnv
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _) => TestEnv.StartApp();

        [AssemblyCleanup]
        public static void AssemblyCleanup() => TestEnv.StopApp();
    }
}
