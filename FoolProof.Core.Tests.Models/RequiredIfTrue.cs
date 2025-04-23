namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfTrue
    {
        public class Model : ValidationModelBase
        {
            public bool? Value1 { get; set; }

            [RequiredIfTrue("Value1")]
            public string? Value2 { get; set; }
        }
    }
}