using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class GreaterThan
    {
        public class DateModel : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThan(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }
        }

        public class Int16Model : ValidationModelBase<GreaterThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThan(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }

        public class TimeModel : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThan(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }
        }

        public class DateTimeModel : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [GreaterThan(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }
        }
    }
}
