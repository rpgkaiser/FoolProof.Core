namespace FoolProof.Core.Tests.E2eTests
{
    public static class Extensions
    {
        public static string? Cap(this string? text)
            => string.IsNullOrWhiteSpace(text) 
                ? text
                : text[0].ToString().ToUpperInvariant() + text[1..];
    }

    public class CustomTestMethodAttribute: TestMethodAttribute
    {
        public CustomTestMethodAttribute()
            : this(null) { }

        public CustomTestMethodAttribute(string? displayName)
            : base(displayName) { }

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var testResults = base.Execute(testMethod);
            var className = testMethod.TestClassName ?? "Unknown";
            className = className.Replace("FoolProof.Core.Tests.E2eTests.", "");
            
            foreach (var testRes in testResults)
                testRes.DisplayName = $"[{className}]: {testRes.DisplayName}";

            return testResults;
        }
    }
}
