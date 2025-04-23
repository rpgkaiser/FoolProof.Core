using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class NotIn
    {
        public class SingleValueModel<T> : ValidationModelBase
        {
            [DataType("StringList")]
            public T? Value1 { get; set; }

            [DataType(DataType.Text)]
            [NotIn(nameof(Value1))]
            public T? Value2 { get; set; }

            [DataType(DataType.Text)]
            [NotIn(nameof(Value1), PassOnNull = true)]
            public T? ValuePwn { get; set; }

            [Display(Description = "DateNotIn: Most be in [01/01/2025, 02/02/2025, 03/03/2025, 04/04/2025, 05/05/2025]")]
            [NotIn<DateOnly>("01/01/2025", "02/02/2025", "03/03/2025", "04/04/2025", "05/05/2025")]
            [DataType(DataType.Date)]
            public DateOnly? DateNotIn { get; set; }

            [Display(Description = "TimeNotIn: Most be in [01:00, 01:30, 02:00, 02:30, 03:00]")]
            [NotIn<TimeSpan>("01:00", "01:30", "02:00", "02:30", "03:00")]
            [DataType(DataType.Time)]
            public TimeSpan? TimeNotIn { get; set; }
        }

        public class DateTimeListModel : ValidationModelBase
        {
            [DataType("DateTimeList")]
            public IEnumerable<DateTime>? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [NotIn(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [NotIn(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }

            [Display(Description = "DateTimeNotIn: Most be in [01/01/2025 01:00, 02/02/2025 02:00, 03/03/2025 03:00, 04/04/2025 04:00, 05/05/2025 05:00]")]
            [NotIn<DateTime>("01/01/2025 01:00", "02/02/2025 02:00", "03/03/2025 03:00", "04/04/2025 04:00", "05/05/2025 05:00")]
            [DataType(DataType.DateTime)]
            public DateTime? DateTimeNotIn { get; set; }
        }

        public class In16ListModel : ValidationModelBase
        {
            [DataType("Int16List")]
            public IEnumerable<Int16>? Value1 { get; set; }

            [NotIn(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [NotIn(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }

            [Display(Description = "ValueNotIn: Most be in [-5, -1, 0, 1, 5]")]
            [NotIn<Int16>(-5, -1, 0, 1, 5)]
            public Int16? ValueNotIn { get; set; }
        }
    }
}
