using System.ComponentModel.DataAnnotations;

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

        public class Int16ModelWithPassNull : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1", PassOnNull = true)]
            public Int16? Value2 { get; set; }
        }

        public class TimeModel : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThanOrEqualTo("Value1")]
            public TimeSpan? Value2 { get; set; }
        }

        public class TimeModelWithPassNull : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThanOrEqualTo("Value1", PassOnNull = true)]
            public TimeSpan? Value2 { get; set; }
        }
    }
}
