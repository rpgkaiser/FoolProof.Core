using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class LessThan
    {
        public class DateModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Date)]
            [LessThan(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Date)]
            [LessThan(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }

            [Display(Description = "MaxDate: Most be less than 01/01/2025")]
            [DataType(DataType.Date)]
            [LessThan<DateOnly>("01/01/2025")]
            public DateOnly? MaxDate { get; set; }
        }

        public class Int16Model : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            public Int16? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [LessThan(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [LessThan(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }

            [Display(Description = "MaxValue: Most be less than 1000")]
            [LessThan<Int16>(1000)]
            public Int16? MaxValue { get; set; }
        }

        public class TimeModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Time)]
            [LessThan(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Time)]
            [LessThan(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }

            [Display(Description = "MaxTime: Most be less than 4h and 30min")]
            [DataType(DataType.Time)]
            [LessThan<TimeSpan>("04:30")]
            public TimeSpan? MaxTime { get; set; }
        }

        public class DateTimeModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.DateTime)]
            [LessThan(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.DateTime)]
            [LessThan(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }

            [Display(Description = "MaxDateTime: Most be less than 01/01/2025 12:00")]
            [DataType(DataType.DateTime)]
            [LessThan<DateTime>("01/01/2025 12:00")]
            public DateTime? MaxDateTime { get; set; }
        }
    }
}
