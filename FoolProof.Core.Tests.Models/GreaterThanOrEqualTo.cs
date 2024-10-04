using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class GreaterThanOrEqualTo
    {
        public class DateModel : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThanOrEqualTo(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }
        }

        public class Int16Model : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThanOrEqualTo(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [GreaterThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }

        public class TimeModel : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThanOrEqualTo(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }
        }

        public class DateTimeModel : ValidationModelBase<GreaterThanOrEqualToAttribute>
        {
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [GreaterThanOrEqualTo(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [GreaterThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }
        }
    }
}
