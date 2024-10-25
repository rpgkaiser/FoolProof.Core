using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class In
    {
        public class SingleValueModel : ValidationModelBase<InAttribute>
        {
            public object? Value1 { get; set; }

            [In(nameof(Value1))]
            public object? Value2 { get; set; }

            [In(nameof(Value1), PassOnNull = true)]
            public object? ValuePwn { get; set; }
        }

        public class DateTimeListModel : ValidationModelBase<InAttribute>
        {
            public IEnumerable<DateTime>? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [In(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [In(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }
        }

        public class In16ListModel : ValidationModelBase<InAttribute>
        {
            public IEnumerable<Int16>? Value1 { get; set; }

            [In(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [In(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }
    }
}
