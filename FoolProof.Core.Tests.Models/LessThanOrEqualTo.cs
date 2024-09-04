namespace FoolProof.Core.Tests.Models
{
    public class LessThanOrEqualTo
    {
        public class DateModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public Int16? Value2 { get; set; }
        }
    }
}
