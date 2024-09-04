namespace FoolProof.Core.Tests.Models
{
    public class RegularExpressionIf
    {
        public class Model : ValidationModelBase<RegularExpressionIfAttribute>
        {
            public bool? Value1 { get; set; }

            [RegularExpressionIf("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$", "Value1", true)]
            public string? Value2 { get; set; }
        }
    }
}
