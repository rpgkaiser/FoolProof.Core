namespace FoolProof.Core.Tests.Models
{
    public class GreaterThanOrEqualTo
    {
        public class DateModel : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassNull : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1")]
            public Int16? Value2 { get; set; }
        }
    }
}
