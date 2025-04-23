using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class GreaterThan
    {
        public class DateModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Date)]
            [GreaterThan(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Date)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }

            [Display(Description = "MinDate: Most be greater than 01/01/2025")]
            [DataType(DataType.Date)]
            [GreaterThan<DateOnly>("01/01/2025")]
            public DateOnly? MinDate { get; set; }
        }

        public class Int16Model : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            public Int16? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [GreaterThan(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }

            [Display(Description = "MinValue: Most be greater than 1000")]
            [GreaterThan<Int16>(1000)]
            public Int16? MinValue { get; set; }
        }

        public class TimeModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Time)]
            [GreaterThan(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Time)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }

            [Display(Description = "MinTime: Most be greater than 4h and 30min")]
            [DataType(DataType.Time)]
            [GreaterThan<TimeSpan>("04:30")]
            public TimeSpan? MinTime { get; set; }
        }

        public class DateTimeModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.DateTime)]
            [GreaterThan(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.DateTime)]
            [GreaterThan(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }

            [Display(Description = "MinDateTime: Most be greater than 01/01/2025 12:00")]
            [DataType(DataType.DateTime)]
            [GreaterThan<DateTime>("01/01/2025 12:00")]
            public DateTime? MinDateTime { get; set; }
        }
    }
}
