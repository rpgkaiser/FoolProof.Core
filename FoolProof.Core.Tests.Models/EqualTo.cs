namespace FoolProof.Core.Tests.Models
{
    public class EqualTo
    {
        public class Model : ValidationModelBase<EqualToAttribute>
        {
            public string? Value1 { get; set; }

            [EqualTo(nameof(Value1))]
            public string? Value2 { get; set; }

            [EqualTo(nameof(Value1), PassOnNull = true)]
            public string? ValuePwn { get; set; }
        }
    }
}
