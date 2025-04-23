using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class EqualTo
    {
        public class Model : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            public string? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [EqualTo(nameof(Value1))]
            public string? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [EqualTo(nameof(Value1), PassOnNull = true)]
            public string? ValuePwn { get; set; }

            [Display(Description = "EmptyValue: Valid if empty")]
            [IsEmpty]
            public string? EmptyValue { get; set; }

            [Display(Description = "TrueValue: Valid if true")]
            [IsTrue]
            public bool? TrueValue { get; set; }

            [Display(Description = "FalseValue: Valid if false")]
            [IsFalse]
            public bool? FalseValue { get; set; }

            [Display(Description = "EqualToValue: Valid if equals to 1000")]
            [EqualTo<Int16>(1000)]
            public Int16? EqualToValue { get; set; }

            [Display(Description = "EqualToDate: Valid if equals to 01/01/2025")]
            [EqualTo<DateOnly>("01/01/2025")]
            [DataType(DataType.Date)]
            public DateOnly? EqualToDate { get; set; }

            [Display(Description = "EqualToTime: Valid if equals to 10:30")]
            [EqualTo<TimeSpan>("10:30")]
            [DataType(DataType.Time)]
            public TimeSpan? EqualToTime { get; set; }

            [Display(Description = "EqualToDateTime: Valid if equals to 01/01/2025 06:30")]
            [EqualTo<DateTime>("01/01/2025 06:30")]
            [DataType(DataType.DateTime)]
            public DateTime? EqualToDateTime { get; set; }
        }
    }
}
