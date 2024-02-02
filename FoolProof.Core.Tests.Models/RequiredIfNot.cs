namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfNot
    {
        public class Model : ValidationModelBase<RequiredIfNotAttribute>
        {
            public string? Value1 { get; set; }

            [RequiredIfNot("Value1", "hello")]
            public string? Value2 { get; set; }
        }
    }
}
