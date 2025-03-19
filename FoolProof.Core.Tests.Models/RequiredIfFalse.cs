namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfFalse
    {
        public class Model : ValidationModelBase
        {
            public bool? Value1 { get; set; }

            [RequiredIfFalse("Value1")]
            public string? Value2 { get; set; }
        }
    }
}
