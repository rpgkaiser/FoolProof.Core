namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfEmpty
    {
        public class Model : ValidationModelBase
        {
            public string? Value1 { get; set; }

            [RequiredIfEmpty("Value1")]
            public string? Value2 { get; set; }
        }
    }
}
