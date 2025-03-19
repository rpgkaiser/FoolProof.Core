using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class NotEqualTo
    {
        public class Model : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            public string? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [NotEqualTo(nameof(Value1))]
            public string? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [NotEqualTo(nameof(Value1), PassOnNull = true)]
            public string? ValuePwn { get; set; }

            [Display(Description = "NotEmptyValue: Valid if not empty")]
            [IsNotEmpty]
            public string? NotEmptyValue { get; set; }

            [Display(Description = "NotEqualToValue: Valid if not equals to 1000")]
            [NotEqualTo<Int16>(1000)]
            public Int16? NotEqualToValue { get; set; }

            [Display(Description = "NotEqualToDate: Valid if not equals to 01/01/2025")]
            [NotEqualTo<DateOnly>("01/01/2025")]
            [DataType(DataType.Date)]
            public DateOnly? NotEqualToDate { get; set; }

            [Display(Description = "NotEqualToTime: Valid if not equals to 10:30")]
            [NotEqualTo<TimeSpan>("10:30")]
            [DataType(DataType.Time)]
            public TimeSpan? NotEqualToTime { get; set; }

            [Display(Description = "NotEqualToDateTime: Valid if not equals to 01/01/2025 06:30")]
            [NotEqualTo<DateTime>("01/01/2025 06:30")]
            [DataType(DataType.DateTime)]
            public DateTime? NotEqualToDateTime { get; set; }
        }
    }
}
