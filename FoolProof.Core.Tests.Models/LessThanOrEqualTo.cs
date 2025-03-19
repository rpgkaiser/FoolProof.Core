using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class LessThanOrEqualTo
    {
        public class DateModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Date)]
            [LessThanOrEqualTo(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Date)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }

            [Display(Description = "MaxDate: Most be less or equals to 01/01/2025")]
            [DataType(DataType.DateTime)]
            [LessThan<DateOnly>("01/01/2025")]
            public DateOnly? MaxDate { get; set; }
        }

        public class Int16Model : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            public Int16? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [LessThanOrEqualTo(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }

            [Display(Description = "MaxValue: Most be less or equals to 1000")]
            [DataType(DataType.DateTime)]
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
            [LessThanOrEqualTo(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Time)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }

            [Display(Description = "MaxTime: Most be less or equals to 4h and 30min")]
            [DataType(DataType.DateTime)]
            [LessThan<DateTime>("04:30")]
            public TimeSpan? MaxTime { get; set; }
        }

        public class DateTimeModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.DateTime)]
            [LessThanOrEqualTo(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.DateTime)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }

            [Display(Description = "MaxDateTime: Most be less or equals to 01/01/2025 12:00")]
            [DataType(DataType.DateTime)]
            [LessThan<DateTime>("01/01/2025 12:00")]
            public DateTime? MaxDateTime { get; set; }
        }
    }
}
