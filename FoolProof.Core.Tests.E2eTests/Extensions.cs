namespace FoolProof.Core.Tests.E2eTests
{
    public static class Extensions
    {
        public static string? Cap(this string? text)
            => string.IsNullOrWhiteSpace(text) 
                ? text
                : text[0].ToString().ToUpperInvariant() + text[1..];
    }
}
