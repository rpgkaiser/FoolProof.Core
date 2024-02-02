namespace FoolProof.Core.Tests.Models
{
    public class EqualTo
    {
        public class Model : ValidationModelBase<EqualToAttribute>
        {
            public string? Value1 { get; set; }

            [EqualTo("Value1")]
            public string? Value2 { get; set; }
        }

        public class ModelWithPassOnNull : ValidationModelBase<EqualToAttribute>
        {
            public string? Value1 { get; set; }

            [EqualTo("Value1", PassOnNull = true)]
            public string? Value2 { get; set; }
        }
    }
}
