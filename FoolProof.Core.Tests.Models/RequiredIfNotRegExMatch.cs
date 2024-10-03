namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfNotRegExMatch
    {
        public class Model : ValidationModelBase<RequiredIfNotRegExMatchAttribute>
        {
            public string? Value1 { get; set; }

            [RequiredIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string? Value2 { get; set; }
        }
    }
}
