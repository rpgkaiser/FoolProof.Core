namespace FoolProof.Core.Tests.Models
{
    public class LessThan
    {
        public class DateModel : ValidationModelBase<LessThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassOnNull : ValidationModelBase<LessThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<LessThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThan("Value1")]
            public Int16? Value2 { get; set; }
        }
    }
}
