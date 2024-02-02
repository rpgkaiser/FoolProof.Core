namespace FoolProof.Core.Tests.Models
{
    public class RequiredIf
    {
        public class Model : ValidationModelBase<RequiredIfAttribute>
        {
            public string? Value1 { get; set; }

            [RequiredIf("Value1", "hello")]
            public string? Value2 { get; set; }
        }

        public class ComplexModel : ValidationModelBase<RequiredIfAttribute>
        {
            public class SubModel
            {
                public string? InnerValue { get; set; }
            }

            public SubModel? Value1 { get; set; }

            [RequiredIf("Value1.InnerValue", "hello")]
            public string? Value2 { get; set; }
        }
    }
}
