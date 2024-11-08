using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class In
    {
        public class SingleValueModel<T> : ValidationModelBase<InAttribute>
        {
            [DataType("StringList")]
            public T? Value1 { get; set; }

            [DataType(DataType.Text)]
            [In(nameof(Value1))]
            public T? Value2 { get; set; }

            [DataType(DataType.Text)]
            [In(nameof(Value1), PassOnNull = true)]
            public T? ValuePwn { get; set; }
        }

        public class DateTimeListModel : ValidationModelBase<InAttribute>
        {
            [DataType("DateTimeList")]
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
            [DataType("Int16List")]
            public IEnumerable<Int16>? Value1 { get; set; }

            [In(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [In(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }
    }
}
