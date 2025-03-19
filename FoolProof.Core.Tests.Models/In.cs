using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class In
    {
        public class SingleValueModel<T> : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType("StringList")]
            public T? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.Text)]
            [In(nameof(Value1))]
            public T? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.Text)]
            [In(nameof(Value1), PassOnNull = true)]
            public T? ValuePwn { get; set; }

            [Display(Description = "DateIn: Most be in [01/01/2025, 02/02/2025, 03/03/2025, 04/04/2025, 05/05/2025]")]
            [In<DateOnly>("01/01/2025", "02/02/2025", "03/03/2025", "04/04/2025", "05/05/2025")]
            [DataType(DataType.Date)]
            public DateOnly? DateIn { get; set; }

            [Display(Description = "TimeIn: Most be in [01:00, 01:30, 02:00, 02:30, 03:00]")]
            [In<TimeSpan>("01:00", "01:30", "02:00", "02:30", "03:00")]
            [DataType(DataType.Time)]
            public TimeSpan? TimeIn { get; set; }
        }

        public class DateTimeListModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType("DateTimeList")]
            public IEnumerable<DateTime>? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [DataType(DataType.DateTime)]
            [In(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [DataType(DataType.DateTime)]
            [In(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }

            [Display(Description = "DateTimeIn: Most be in [01/01/2025 01:00, 02/02/2025 02:00, 03/03/2025 03:00, 04/04/2025 04:00, 05/05/2025 05:00]")]
            [In<DateTime>("01/01/2025 01:00", "02/02/2025 02:00", "03/03/2025 03:00", "04/04/2025 04:00", "05/05/2025 05:00")]
            [DataType(DataType.DateTime)]
            public DateTime? DateTimeIn { get; set; }
        }

        public class In16ListModel : ValidationModelBase
        {
            [Display(Description = Value1Description)]
            [DataType("Int16List")]
            public IEnumerable<Int16>? Value1 { get; set; }

            [Display(Description = Value2Description)]
            [In(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [Display(Description = ValuePwnDescription)]
            [In(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }

            [Display(Description = "ValueIn: Most be in [-5, -1, 0, 1, 5]")]
            [In<Int16>(-5, -1, 0, 1, 5)]
            public Int16? ValueIn { get; set; }
        }
    }
}
